using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.Mediator;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.ExcluirVendedorCadastroRejeitado;
using System;

namespace MinhaLoja.Worker.UnapprovedRegistrationSellers
{
    public class ExclusaoCadastroNaoAprovadoVendedorFunction
    {
        private readonly IServiceProvider _serviceProvider;

        public ExclusaoCadastroNaoAprovadoVendedorFunction(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [Function("ExclusaoCadastroNaoAprovadoVendedorFunction")]
        public async void Run(
            [TimerTrigger("0 30 9 * * *")] //0/29 * * * * *
            FunctionContext context)
        {
            var logger = context.GetLogger("ExclusaoCadastroNaoAprovadoVendedorFunction");
            logger.LogInformation($"{DateTime.Now}");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var mediatorHandler = (IMediatorHandler)scope.ServiceProvider.GetService(typeof(IMediatorHandler));
                var request = new ExcluirVendedoresCadastroRejeitadoRequest();
                var response = (IResponseAppService<bool>)await mediatorHandler.SendRequestToHandlerAsync(request);

                logger.LogInformation($"Sucesso exclusões: {response.Success}");
            }
        }
    }
}
