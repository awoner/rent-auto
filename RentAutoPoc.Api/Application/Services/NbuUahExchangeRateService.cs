using RentAutoPoc.Api.Services;

namespace RentAutoPoc.Api.Application.Services;

public interface IHryvnaExchangeRateService
{
    Task<RateDto> GetCurrentAsync(string currencyCode, CancellationToken cancellationToken);
}

public class NbuUahExchangeRateService : IHryvnaExchangeRateService
{
    private readonly NbuClient _client;

    public NbuUahExchangeRateService(NbuClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<RateDto> GetCurrentAsync(string currencyCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            return new RateDto { Amount = 0 };
        }

        if (currencyCode.Equals("UAH", StringComparison.InvariantCultureIgnoreCase))
        {
            return new RateDto { Amount = 1 };
        }

        var rates = await _client.GetCurrentUahRatesAsync(cancellationToken);
        var targetRate = rates.FirstOrDefault(r => r.CurrencyCodeL.Equals(currencyCode, StringComparison.InvariantCultureIgnoreCase));

        return new RateDto
        {
            Amount = targetRate?.Amount ?? 0
        };
    }
}