using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MinhaLoja.Core.Settings;
using MinhaLoja.Infra.Ioc;
using System;

namespace MinhaLoja.Worker.UnapprovedRegistrationSellers
{
    //TODO: erros globais
    public class Program
    {
        public static void Main()
        {
            IConfiguration configuration = null;
            var host = new HostBuilder()
                .ConfigureAppConfiguration(configure =>
                {
                    configuration = 
                        configure
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();
                })
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((services) =>
                {
                    string environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
                    IConfigurationSection configurationSection = configuration.GetSection(nameof(GlobalSettings));

                    GlobalSettings globalSettings = services.LoadGlobalSettings(
                        configurationSection,
                        environmentName);

                    Dependencies.RegisterDependencies(services, globalSettings);
                })
                .Build();

            host.Run();
        }
    }
}