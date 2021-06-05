using Microsoft.AspNetCore.Builder;

namespace MinhaLoja.Infra.Api.StartupConfigurations
{
    public static class RequestLocalization
    {
        public static void UseRequestLocalizationDefault(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("pt-BR"));
        }
    }
}
