using System.Reflection;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;

namespace RentAutoPoc.Api
{
    internal static class Program
    {
        public static IMetricsRoot Metrics { get; set; }

        public static async Task Main(string[] args)
        {
            SetTitle();

            try
            {
                using var host = BuildHost(args);
                await host.RunAsync();
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
        }

        private static IHost BuildHost(string[] args)
        {
            Metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Build();
            
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
                })
                .ConfigureMetrics(Metrics)
                .UseMetrics(
                    options =>
                    {
                        options.EndpointOptions = endpointsOptions =>
                        {
                            endpointsOptions.MetricsTextEndpointOutputFormatter = Metrics.OutputMetricsFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First();
                            endpointsOptions.MetricsEndpointOutputFormatter = Metrics.OutputMetricsFormatters.OfType<MetricsPrometheusProtobufOutputFormatter>().First();
                        };
                    })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                    options.ValidateOnBuild = true;
                })
                .Build();
        }

        private static void SetTitle()
        {
            var title = $"Uklon.DriverGateway {Assembly.GetExecutingAssembly().GetName().Version} {Assembly.GetExecutingAssembly().Location}";
            Console.Title = title;
        }
    }
}