using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.AprovacaoCadastro;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.AprovarCadastro
{
    public class AprovarCadastroUsuarioVendedorAppService : AppService<bool>,
        IRequestHandler<AprovarCadastroUsuarioVendedorRequest, IResponseAppService<bool>>
    {
        private readonly IVendedorRepository _vendedorRepository;

        public AprovarCadastroUsuarioVendedorAppService(
            IVendedorRepository vendedorRepository,
            DependenciesAppService dependenciesAppService)
            : base(dependenciesAppService)
        {
            _vendedorRepository = vendedorRepository;
        }

        public async Task<IResponseAppService<bool>> Handle(
            AprovarCadastroUsuarioVendedorRequest request, 
            CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
                return ReturnNotifications(request.Notifications);

            Entities.Vendedor vendedor =
                _vendedorRepository
                    .GetEntity(asNoTracking: false)
                    .FirstOrDefault(vendedor => vendedor.Id == request.IdVendedor);

            if (vendedor == null)
                return ReturnNotification(nameof(request.IdVendedor), MensagensVendedor.Vendedor_Aprovar_NotificacaoUsuarioInexistente);

            vendedor.AprovarCadastro();
            if (vendedor.IsValid == false)
                return ReturnNotification(nameof(vendedor.CadastroAprovado), MensagensVendedor.Vendedor_Aprovar_NotificacaoErroAprovacao);


            if (await CommitAsync())
            {
                await PublishEventAsync(
                    @event: new CadastroVendedorAprovadoEvent(
                        idVendedor: vendedor.Id,
                        email: vendedor.Email
                    ),
                    aggregateRoot: vendedor
                );
                
                return ReturnSuccess();
            }

            throw new DomainException("erro na realização da Aprovação do Cadastro do Vendedor");
        }
    }
}
