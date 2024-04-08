using System.Reflection;

namespace RentAutoPoc.Api
{
    internal static class Program
    {
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
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
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