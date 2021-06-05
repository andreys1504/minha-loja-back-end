using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhaLoja.Core.Settings;

namespace MinhaLoja.Infra.Api.StartupConfigurations
{
    public static class GlobalSettingsConfiguration
    {
        public static GlobalSettings LoadGlobalSettings(
            this IServiceCollection services,
            IConfigurationSection configurationSection,
            string environmentName)
        {
            return Ioc.GlobalSettingsConfiguration.LoadGlobalSettings(
                services,
                configurationSection,
                environmentName);
        }
    }
}
