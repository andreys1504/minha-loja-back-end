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
                    options.SaveToken = true;
                    options.IncludeErrorDetails = globalSettings.Environment != Environments.Production;
                    options.RequireHttpsMetadata = !globalSettings.NotUseHttps;
                    TimeSpan cacheTime = TimeSpan.FromMinutes(15);
                    JwkOptions JwkOptions = null;

                    if (string.IsNullOrWhiteSpace(globalSettings.Identity.Issuer) == false)
                    {
                        JwkOptions = new JwkOptions(
                                issuer: globalSettings.Identity.Issuer,
                                jwksUri: globalSettings.Identity.JwksUri,
                                cacheTime: cacheTime);
                    }
                    else
                    {
                        JwkOptions = new JwkOptions(
                                jwksUri: globalSettings.Identity.JwksUri,
                                cacheTime: cacheTime);
                    }

                    options.SetJwksOptions(JwkOptions);
                });
        }
    }
}
