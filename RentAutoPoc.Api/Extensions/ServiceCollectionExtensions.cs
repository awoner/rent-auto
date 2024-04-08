using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using NCrontab;
using Nest;
using RentAutoPoc.Api.Application;
using RentAutoPoc.Api.Application.Services;
using RentAutoPoc.Api.Infrastructure.Crons;
using RentAutoPoc.Api.Services;
using RentAutoPoc.Api.Settings;

namespace RentAutoPoc.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAppServices(this IServiceCollection services,
        ConnectionStrings connectionStrings)
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

        services.AddSingleton<IGampClient>(_ =>
            new GampClient(
                userId: "123",
                clientId: "207438",
                apiSecret: "Sm5biFbgSFuDCnpze8mVbA",
                measurementId: "G-CE14B9L6T5"));
        services.AddSingleton<IHryvnaExchangeRateService, NbuUahExchangeRateService>();
        services.AddSingleton<NbuClient>();

        services.AddCronJob<NbuMetricsCronJob>("0 * * * *");
        
        return services;
    }
    
    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, string cronExpression)
        where T : class, ICronJob
    {
        var cron = CrontabSchedule.TryParse(cronExpression)
                   ?? throw new ArgumentException("Invalid cron expression", nameof(cronExpression));

        var entry = new CronRegistryEntry(typeof(T), cron);

        services.AddHostedService<CronScheduler>();
        services.TryAddSingleton<T>();
        services.AddSingleton(entry);

        return services;
    }
}