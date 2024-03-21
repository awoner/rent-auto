using MongoDB.Driver;
using Nest;
using RentAutoPoc.Api.Settings;

namespace RentAutoPoc.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAppServices(this IServiceCollection services, ConnectionStrings connectionStrings)
    {
        services.AddSingleton(_ =>
        {
            var client = new MongoClient(connectionStrings.MongoConnectionStrings.ConnectionString);
            return client.GetDatabase(connectionStrings.MongoConnectionStrings.DatabaseName);
        });
        
        
        services.AddSingleton<IElasticClient>(sp =>
        {
            var settings = new ConnectionSettings(new Uri(connectionStrings.ElasticsearchConnectionStrings.Url));
            return new ElasticClient(settings);
        });
        
        return services;
    }
}
