using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MinhaLoja.Core.Infra.Identity.Middlewares;
using MinhaLoja.Core.Settings;
using MinhaLoja.Infra.Api.StartupConfigurations;
using NetDevPack.Security.JwtSigningCredentials.AspNetCore;

namespace MinhaLoja.Api.Identity
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private GlobalSettings _globalSettings;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var webHostEnvironment = services.GetServiceInConfigureServices<IWebHostEnvironment>();
            IConfigurationSection configurationSection = _configuration.GetSection(nameof(GlobalSettings));

            _globalSettings = services.LoadGlobalSettings(
                configurationSection,
                webHostEnvironment.EnvironmentName);
            services.RegisterDependencies(_globalSettings);

            services.AddCors();
            services.AddControllers();
            services.AddMemoryCache();
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "MinhaLoja.Api.Identity", Version = "v1" });
            });
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
                app.UseSwagger();
                app.UseSwaggerUI(setup => setup.SwaggerEndpoint("/swagger/v1/swagger.json", "MinhaLoja.Api.Identity v1"));
            }
            else
            {
                app.UseExceptionHandlerApplication();
            }

            if (_globalSettings.NotUseHttps == false)
            {
                app.UseHttpsRedirection();
            }
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
