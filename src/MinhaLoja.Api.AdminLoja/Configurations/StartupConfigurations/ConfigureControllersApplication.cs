using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace MinhaLoja.Api.AdminLoja.Configurations.StartupConfigurations
{
    public static class ConfigureControllersApplication
    {
        public static IMvcBuilder AddControllersCustom(this IServiceCollection services)
        {
            return MvcServiceCollectionExtensions.AddControllers(
                services, 
                configure => AddFilterAuthorizationControllers(configure)
            );
        }

        private static void AddFilterAuthorizationControllers(MvcOptions mvcOptions)
        {
            var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

            mvcOptions.Filters.Add(new AuthorizeFilter(policy));
        }
    }
}
