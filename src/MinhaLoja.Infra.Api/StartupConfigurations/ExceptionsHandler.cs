using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Infra.Services.LogHandler;
using MinhaLoja.Core.Infra.Services.LogHandler.Models;
using MinhaLoja.Core.Settings;

namespace MinhaLoja.Infra.Api.StartupConfigurations
{
    public static class ExceptionsHandler
    {
        public static IApplicationBuilder UseExceptionHandlerApplication(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(configure =>
            {
                configure.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    var errorHandler = (ILogErrorHandler)context.RequestServices.GetService(typeof(ILogErrorHandler));
                    var globalSettings = (GlobalSettings)context.RequestServices.GetService(typeof(GlobalSettings));
                    var identityService = (IIdentityService)context.RequestServices.GetService(typeof(IIdentityService));

                    string userId = identityService.GetUserId(context.User);

                    var error = new ErrorApplicationModel
                    {
                        Application = globalSettings.CurrentApplication,
                        ExceptionTitle = exceptionHandlerPathFeature.Error.Message,
                        Error = exceptionHandlerPathFeature.Error.StackTrace,
                        Path = exceptionHandlerPathFeature.Path,
                        UserId = userId
                    };
                    await errorHandler.SendAsync(error: error);

                    context.Response.ContentType = "text/html";

                    await context.Response.WriteAsync("<html lang=\"pt-BR\"><body>\r\n");
                    await context.Response.WriteAsync("ERRO!<br><br>\r\n");
                    await context.Response.WriteAsync("</body></html>\r\n");
                });
            });

            return app;
        }
    }
}
