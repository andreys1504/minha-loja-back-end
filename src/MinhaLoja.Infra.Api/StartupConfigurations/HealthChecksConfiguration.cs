using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MinhaLoja.Core.Settings;
using Newtonsoft.Json;
using System.Linq;

namespace MinhaLoja.Infra.Api.StartupConfigurations
{
    public static class HealthChecksConfiguration
    {
        public static void AddHealthChecksCustom(
            this IServiceCollection services,
            GlobalSettings globalSettings)
        {
            services
                .AddHealthChecks()
                .AddSqlServer(globalSettings.DatabaseConnectionString, name: "baseSqlServer")
                .AddMongoDb(globalSettings.DatabaseSecondaryConnectionString, name: "baseMongoDb")
                .AddRedis(globalSettings.DatabaseCacheConnectionString, name: "cacheRedis");
        }

        public static void UseHealthChecksCustom(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/status", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        Status = report.Status.ToString(),
                        HealthChecks = report.Entries.Select(x => new
                        {
                            Components = x.Key,
                            Status = x.Value.Status.ToString(),
                            x.Value.Description
                        }),
                        HealthCheckDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });
        }
    }
}
