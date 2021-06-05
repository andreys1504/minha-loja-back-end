using Microsoft.Extensions.DependencyInjection;
using MinhaLoja.Core.Settings;

namespace MinhaLoja.Infra.Api.StartupConfigurations
{
    public static class DependenciesConfiguration
    {
        public static void RegisterDependencies(
            this IServiceCollection services,
            GlobalSettings settings)
        {
            Ioc.Dependencies.RegisterDependenciesWithIdentity(services, settings);
        }
    }
}
