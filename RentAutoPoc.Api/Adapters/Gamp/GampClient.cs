using System.Text;
using System.Text.Json;
using RentAutoPoc.Api.Infrastructure;

namespace RentAutoPoc.Api.Services;

public interface IGampClient
{
    Task<bool> SendEvents<T>(params GampEvent<T>[] events) where T : GampEventParams;
}

public class GampClient : IGampClient
{
    private readonly string _clientId;
    private readonly string _userId;
    private readonly string _apiSecret;
    private readonly string _measurementId;

    private readonly HttpClient _httpClient;
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
        WriteIndented = true
    };

    public GampClient(string clientId, string userId, string apiSecret, string measurementId)
    {
        _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        _userId = userId ?? throw new ArgumentNullException(nameof(userId));
        _apiSecret = apiSecret ?? throw new ArgumentNullException(nameof(apiSecret));
        _measurementId = measurementId ?? throw new ArgumentNullException(nameof(measurementId));
        
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://www.google-analytics.com/mp/collect"),
        };
    }

    public async Task<bool> SendEvents<T>(params GampEvent<T>[] events) where T : GampEventParams
    {
        if (!events.Any())
        {
            throw new ArgumentException($"Argument {nameof(events)} cannot be empty.");
        }
        
        var request = new GampRequest<T>
        {
            ClientId = _clientId,
            UserId = _userId,
            NonPersonalizedAds = false,
            Events =  events,
        };

        var response = await _httpClient.PostAsync(GetQueryParams(), CreateContent(request));
        
        return response.IsSuccessStatusCode;
    }

    private static StringContent CreateContent<T>(GampRequest<T> request) where T : GampEventParams
    {
        var payload = JsonSerializer.Serialize(request, JsonSerializerOptions);
        return new StringContent(payload, Encoding.UTF8, "text/plain");
    }

    private string GetQueryParams() =>
        $"?api_secret={_apiSecret}&measurement_id={_measurementId}";
}