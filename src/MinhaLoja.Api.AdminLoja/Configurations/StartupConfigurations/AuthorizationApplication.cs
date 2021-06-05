using Microsoft.Extensions.DependencyInjection;
using MinhaLoja.Core.Authorizations;

namespace MinhaLoja.Api.AdminLoja.Configurations.StartupConfigurations
{
    public static class AuthorizationApplication
    {
        public static IServiceCollection AddAuthorizationApplication(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                AuthorizationsApplications.AdminLoja.GetAll.ForEach(authorization =>
                {
                    options.AddPolicy(authorization, policy => policy.RequireClaim(authorization, "true"));
                });
            });

            return services;
        }
    }
}
