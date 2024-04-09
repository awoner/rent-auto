using System.Text;
using Newtonsoft.Json;
using RentAutoPoc.Api.Application.Services;
using RentAutoPoc.Api.Infrastructure.Crons;
using RentAutoPoc.Api.Services;

namespace RentAutoPoc.Api.Application;

public class NbuMetricsCronJob : ICronJob
{
    private readonly IGampClient _gampClient;
    private readonly IHryvnaExchangeRateService _uahExchangeRateService;

    public NbuMetricsCronJob(IGampClient gampClient, IHryvnaExchangeRateService exchangeRateService)
    {
        _gampClient = gampClient ?? throw new ArgumentNullException(nameof(gampClient));
        _uahExchangeRateService = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var rate = await _uahExchangeRateService.GetCurrentAsync("USD", cancellationToken);

            var @event = new GampEvent<CurrencyParams>
            {
                Name = "currency_event",
                Params = new CurrencyParams
                {
                    CurrencyRate = (float)rate.Amount,
                    SessionId = "123",
                    EngagementTimeMSec = 100,
                    DebugMode = 1,
                }
            };

            await _gampClient.SendEvents(@event);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}