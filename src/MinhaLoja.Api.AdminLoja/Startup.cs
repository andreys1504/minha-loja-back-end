using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MinhaLoja.Api.AdminLoja.Configurations.StartupConfigurations;
using MinhaLoja.Core.Infra.Identity.Middlewares;
using MinhaLoja.Core.Settings;
using MinhaLoja.Infra.Api.StartupConfigurations;

namespace MinhaLoja.Api.AdminLoja
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private GlobalSettings _globalSettings;
        private readonly string keyCacheConnectionString = "ApiSettings:Cache:ConnectionString";
        private readonly string keyCacheInstanceName = "ApiSettings:Cache:InstanceName";
        private readonly string keyApplicationInsightsConnectionString = "ApiSettings:ApplicationInsights:ConnectionString";

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
            services.AddControllersCustom();
            services.AddMemoryCache();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = _configuration.GetSection(keyCacheConnectionString).Value;
                options.InstanceName = _configuration.GetSection(keyCacheInstanceName).Value;
            });
            //services.AddAuthorizationApplication(); //trabalhar com Policy
            services.AddAuthenticationCustom(_globalSettings);
            services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Api.AdminLoja", Version = "v1" }));
            services.AddApplicationInsightsTelemetry(options => options.ConnectionString = _configuration.GetSection(keyApplicationInsightsConnectionString).Value);
            services.AddHealthChecksCustom(_globalSettings);

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
                app.UseSwaggerUI(setup => setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Api.AdminLoja v1"));
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHealthChecksCustom();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
