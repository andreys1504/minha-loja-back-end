using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinhaLoja.Core.Settings;
using MinhaLoja.Infra.Ioc;
using System;

namespace MinhaLoja.Worker.DomainEventsReceiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    string environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
                    IConfigurationSection configurationSection = configuration.GetSection(nameof(GlobalSettings));

                    GlobalSettings globalSettings = services.LoadGlobalSettings(
                        configurationSection,
                        environmentName);

                    Dependencies.RegisterDependencies(services, globalSettings);


                    services.AddHostedService<Worker>();
                    services.AddApplicationInsightsTelemetryWorkerService(options =>
                        options.ConnectionString = configuration.GetSection("WorkerSettings:ApplicationInsights:ConnectionString").Value);
                });
    }
}
