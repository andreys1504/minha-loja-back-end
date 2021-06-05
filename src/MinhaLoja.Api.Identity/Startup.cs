using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinhaLoja.Core.Infra.Identity.Middlewares;
using MinhaLoja.Core.Settings;
using MinhaLoja.Infra.Api.StartupConfigurations;
using NetDevPack.Security.JwtSigningCredentials.AspNetCore;

namespace MinhaLoja.Api.Identity
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var webHostEnvironment = services.GetServiceInConfigureServices<IWebHostEnvironment>();
            IConfigurationSection configurationSection = _configuration.GetSection(nameof(GlobalSettings));

            GlobalSettings globalSettings = services.LoadGlobalSettings(
                configurationSection,
                webHostEnvironment.EnvironmentName);
            services.RegisterDependencies(globalSettings);

            services.AddCors();
            services.AddControllers();
            services.AddMemoryCache();
            services.AddApplicationInsightsTelemetry(options => options.ConnectionString = _configuration.GetSection("ApiSettings:ApplicationInsights:ConnectionString").Value);

            var authenticationMiddleware = services.GetServiceInConfigureServices<IAuthenticationMiddleware>();
            authenticationMiddleware.AddAuthenticationApplication(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(cors => cors
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                );
            }
            else
                app.UseExceptionHandlerApplication();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseRequestLocalizationDefault();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseJwksDiscovery();
        }
    }
}
