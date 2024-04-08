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

            var @event = new GampEvent
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

    static async Task SendDataToGoogleAnalytics(RateDto rate)
    {
        var baseUrl = "https://www.google-analytics.com/mp/collect";
        var payload = new
        {
            client_id = "207439",
            user_id = "7777",
            non_personalized_ads = false,
            events = new[]
            {
                new
                {
                    name = "currency_event",
                    currency_rate = rate.Amount,
                    value = rate.Amount,
                    session_id = 1709293625,
                    debug_mode = 1,
                    engagement_time_msec = 100,
                    @params = new
                    {
                        currency_rate = rate.Amount,
                        value = rate.Amount,
                        session_id = 1709293625,
                        debug_mode = 1,
                        engagement_time_msec = 100,
                    },
                }
            }
        };

        var jsonPayload = JsonConvert.SerializeObject(payload);

        using var client = new HttpClient();
        
        client.BaseAddress = new Uri(baseUrl);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var measurementId = "G-CE14B9L6T5";
        var apiSecret = "Sm5biFbgSFuDCnpze8mVbA";

        var response = await client.PostAsync($"?measurement_id={measurementId}&api_secret={apiSecret}", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Data sent successfully to Google Analytics GA4.");
        }
        else
        {
            Console.WriteLine($"Failed to send data to Google Analytics GA4. Status code: {response.StatusCode}");
        }
    }
}