using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using MinhaLoja.Core.Settings;
using NetDevPack.Security.JwtExtensions;
using System;

namespace MinhaLoja.Infra.Api.StartupConfigurations
{
    public static class AuthenticationApplication
    {
        public static void AddAuthenticationCustom(
            this IServiceCollection services,
            GlobalSettings globalSettings)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.IncludeErrorDetails = globalSettings.Environment != Environments.Production;
                    options.SetJwksOptions(new JwkOptions(
                            jwksUri: globalSettings.Identity.JwksUri,
                            cacheTime: TimeSpan.FromMinutes(15)
                        )
                    );
                });
        }
    }
}
