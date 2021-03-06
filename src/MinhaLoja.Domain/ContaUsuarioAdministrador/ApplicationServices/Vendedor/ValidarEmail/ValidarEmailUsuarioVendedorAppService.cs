using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
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
        IRequestHandler<ValidarEmailUsuarioVendedorRequest, IResponseAppService<IList<string>>>
    {
        private readonly IVendedorRepository _vendedorRepository;

        public ValidarEmailUsuarioVendedorAppService(
            IVendedorRepository vendedorRepository,
            DependenciesAppService dependenciesAppService)
            : base(dependenciesAppService)
        {
            _vendedorRepository = vendedorRepository;
        }

        public async Task<IResponseAppService<IList<string>>> Handle(
            ValidarEmailUsuarioVendedorRequest request,
            CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
            {
                return ReturnNotifications(request.Notifications);
            }

            Entities.Vendedor vendedor =
                _vendedorRepository
                    .GetEntity(asNoTracking: false)
                    .FirstOrDefault(VendedorQueries.CodigoValidacaoEmailValido(request.Codigo));

            if (vendedor == null)
            {
                return ReturnNotification(nameof(request.Codigo), MensagensVendedor.Vendedor_ValidarEmail_CodigoIsNotNullOrWhiteSpace);
            }

            if (VendedorQueries.PermissaoValidacaoEmail().Compile()(vendedor) == false)
            {
                vendedor.GerarNovoCodigoValidacaoEmail();
                if (vendedor.IsValid == false)
                {
                    return ReturnNotifications(vendedor.Notifications);
                }

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

                    return ReturnNotification(nameof(request.Codigo), MensagensVendedor.Vendedor_ValidacaoEmail_DataValidacaoVencida);
                }

                throw new DomainException("validação e-mail vendedor: E-mail não validado; Erro na geração do código de validação do E-mail");
            }

            vendedor.SetarEmailValido();

            if (vendedor.IsValid == false)
            {
                return ReturnNotifications(vendedor.Notifications);
            }

            if (await CommitAsync())
            {
                await PublishEventAsync(
                    @event: new EmailUsuarioVendedorValidadoEvent(
                        idVendedor: vendedor.Id
                    ),
                    aggregateRoot: vendedor
                );

                var mensagensRetorno = new List<string>
                {
                    MensagensVendedor.Vendedor_ValidarEmail_Sucesso01,
                    MensagensVendedor.Vendedor_ValidarEmail_Sucesso02
                };

                return ReturnData(mensagensRetorno);
            }

            throw new DomainException("erro na realização da validação do E-mail do Vendedor");
        }
    }
}
