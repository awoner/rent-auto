using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace RentAutoPoc.Api.Extensions;

public static class HttpClientExtensions
{
    public static HttpContent ToHttpContent(this object source) =>
        new StringContent(JsonConvert.SerializeObject(source), Encoding.UTF8, MediaTypeNames.Application.Json);

    public static HttpContent EmptyHttpContent =>
        new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);

    public static async Task<T> FromHttpContentAsync<T>(this HttpContent source)
    {
        var stringResult = await source.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(stringResult);
    }

    public static bool IsUrl(this string urlStr) =>
        Uri.TryCreate(urlStr, UriKind.Absolute, out var uriResult) &&
        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
}