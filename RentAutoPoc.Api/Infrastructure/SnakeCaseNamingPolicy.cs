using System.Text.Json;
using RentAutoPoc.Api.Extensions;

namespace RentAutoPoc.Api.Infrastructure;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

    public override string ConvertName(string name) =>
        name.ToSnakeCase();
}
