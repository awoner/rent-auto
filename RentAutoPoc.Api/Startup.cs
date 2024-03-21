using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Prometheus;
using RentAutoPoc.Api.Extensions;
using RentAutoPoc.Api.Settings;

namespace RentAutoPoc.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var settings = _configuration.Get<AppSettings>();
            
            services.AddResponseCaching()
                .AddResponseCompression(options =>
                {
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                    {
                        "application/vnd.google-earth.kml+xml",
                    });

                    options.EnableForHttps = true;
                })
                .AddControllers()
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .SetIsOriginAllowed(_ => true));
            });
            
            services.ConfigureAppServices(settings.ConnectionStrings);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMetricServer();
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors("CorsPolicy");
            app.UseResponseCompression();
            app.UseRouting();
            app.UseHttpMetrics(options=>
            {
                options.AddCustomLabel("host", context => context.Request.Host.Host);
            });
            
            app.UseResponseCaching();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseStaticFiles();
        }
    }
}
