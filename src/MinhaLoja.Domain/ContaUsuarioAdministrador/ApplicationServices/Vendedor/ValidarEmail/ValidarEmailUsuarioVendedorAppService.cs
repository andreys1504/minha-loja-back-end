using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.ValidacaoEmail;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Queries;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.ValidarEmail
{
    public class ValidarEmailUsuarioVendedorAppService : AppService<IList<string>>,
        IRequestHandler<ValidarEmailUsuarioVendedorRequest, IResponseService<IList<string>>>
    {
        private readonly IVendedorRepository _vendedorRepository;

        public ValidarEmailUsuarioVendedorAppService(
            IVendedorRepository vendedorRepository,
            DependenciesAppService dependenciesAppService) 
            : base(dependenciesAppService)
        {
            _vendedorRepository = vendedorRepository;
        }

        public async Task<IResponseService<IList<string>>> Handle(
            ValidarEmailUsuarioVendedorRequest request, 
            CancellationToken cancellationToken)
        {
            if (request.Validate() is false)
                return ReturnNotifications(request.Notifications);

            Entities.Vendedor vendedor =
                _vendedorRepository
                    .GetEntity(asNoTracking: false)
                    .FirstOrDefault(VendedorQueries.CodigoValidacaoEmailValido(request.Codigo));

            if (vendedor == null)
                return ReturnNotification(nameof(request.Codigo), MensagensVendedor.Vendedor_ValidarEmail_CodigoIsNotNullOrWhiteSpace);

            if(vendedor.PermissaoValidacaoEmail() is false)
            {
                vendedor.GerarNovoCodigoValidacaoEmail();
                if (vendedor.IsValid is false)
                    return ReturnNotifications(vendedor.Notifications);

                if (await CommitAsync())
                {
                    await PublishEventAsync( 
                        @event: new GeradoNovoCodigoValidacaoEmailVendedorEvent(
                            idUsuarioAdministrador: vendedor.UsuarioId,
                            idVendedor: vendedor.Id,
                            emailVendedor: vendedor.Email,
                            codigoValidacaoEmail: vendedor.CodigoValidacaoEmail
                        ),
                        aggregateRoot: vendedor
                    );
                }
                
                return ReturnNotification(nameof(request.Codigo), MensagensVendedor.Vendedor_ValidacaoEmail_DataValidacaoVencida);
            }

            vendedor.SetarEmailValido();

            if (vendedor.IsValid is false)
                return ReturnNotifications(vendedor.Notifications);

            if (await CommitAsync())
            {
                await PublishEventAsync(
                    @event: new EmailUsuarioVendedorValidadoEvent(
                        idVendedor: vendedor.Id
                    ),
                    aggregateRoot: vendedor
                );
            }

            var mensagensRetorno = new List<string>
            {
                MensagensVendedor.Vendedor_ValidarEmail_Sucesso01,
                MensagensVendedor.Vendedor_ValidarEmail_Sucesso02
            };

            return ReturnData(mensagensRetorno);
        }
    }
}
