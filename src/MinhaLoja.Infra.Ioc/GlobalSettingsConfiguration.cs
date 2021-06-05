using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinhaLoja.Core.Settings;

namespace MinhaLoja.Infra.Ioc
{
    public static class GlobalSettingsConfiguration
    {
        public static GlobalSettings LoadGlobalSettings(
            this IServiceCollection services,
            IConfigurationSection configurationSection,
            string environmentName)
        {
            var globalSettings = new GlobalSettings();
            new ConfigureFromConfigurationOptions<GlobalSettings>(configurationSection).Configure(globalSettings);
            globalSettings.SetEnvironment(environmentName);

            services.AddSingleton(globalSettings);

            return globalSettings;
        }
    }
}
