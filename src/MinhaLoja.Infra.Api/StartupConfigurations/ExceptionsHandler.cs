using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using MinhaLoja.Core.Domain.Exceptions;
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
                    var claimService = (IClaimService)context.RequestServices.GetService(typeof(IClaimService));

                    string userId = claimService.GetUserId(context.User);

                    var error = new ErrorApplicationModel
                    {
                        Application = globalSettings.CurrentApplication,
                        ExceptionTitle = exceptionHandlerPathFeature.Error.Message,
                        Error = exceptionHandlerPathFeature.Error.StackTrace,
                        Path = exceptionHandlerPathFeature.Path,
                        UserId = userId
                    };
                    //await errorHandler.SendAsync(error);




                    string message = "Ocorreu um erro na execução da requisição";
                    if (exceptionHandlerPathFeature.Error is DomainException)
                    {
                        message = exceptionHandlerPathFeature.Error.Message;
                    }
                    else
                    {
                        if (globalSettings.Environment != Environments.Production)
                        {
                            message = exceptionHandlerPathFeature.Error.Message;
                        }
                    }

                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(message);
                });
            });

            return app;
        }
    }
}
