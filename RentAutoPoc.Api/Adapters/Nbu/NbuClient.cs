using RentAutoPoc.Api.Extensions;

namespace RentAutoPoc.Api.Services;

public class NbuClient
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://bank.gov.ua/"),
    };

    public async Task<IEnumerable<NbuRateDto>> GetCurrentUahRatesAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync("NBU_Exchange/exchange?json", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<NbuRateDto>();
        }
        
        return await response.Content.FromHttpContentAsync<IEnumerable<NbuRateDto>>();
    }
}