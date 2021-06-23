using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.ExcluirVendedoresCadastroRejeitado;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Queries;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.ExcluirVendedorCadastroRejeitado
{
    public class ExcluirVendedoresCadastroRejeitadoAppService : AppService<bool>,
        IRequestHandler<ExcluirVendedoresCadastroRejeitadoRequest, IResponseAppService<bool>>
    {
        private readonly IUsuarioAdministradorRepository _usuarioAdministradorRepository;
        private readonly IVendedorRepository _vendedorRepository;

        public ExcluirVendedoresCadastroRejeitadoAppService(
            IUsuarioAdministradorRepository usuarioAdministradorRepository,
            IVendedorRepository vendedorRepository,
            DependenciesAppService dependenciesAppService) 
            : base(dependenciesAppService)
        {
            _usuarioAdministradorRepository = usuarioAdministradorRepository;
            _vendedorRepository = vendedorRepository;
        }

        public async Task<IResponseAppService<bool>> Handle(
            ExcluirVendedoresCadastroRejeitadoRequest request, 
            CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
                return ReturnNotifications(request.Notifications);


            IEnumerable<Entities.UsuarioAdministrador> usuarios =
                _usuarioAdministradorRepository
                    .GetEntity(asNoTracking: false)
                    .Include(usuario => usuario.Vendedor)
                    .Where(UsuarioAdministradorQueries.PermitirExclusaoVendedorPorCadastroNaoAprovado())
                    .ToList();

            if (usuarios?.Count() < 1)
                return ReturnSuccess();

            foreach (var usuario in usuarios)
            {
                await _vendedorRepository.DeleteEntityByIdAsync(usuario.Vendedor.Id);
                await _usuarioAdministradorRepository.DeleteEntityByIdAsync(usuario.Id);
            }

            if (await CommitAsync())
            {
                foreach (var usuario in usuarios)
                    await PublishEventAsync(
                        @event: new VendedoresExcluidosPorCadastroRejeitadoEvent(
                            idVendedor: usuario.Id
                        ),
                        aggregateRoot: usuario
                    );

                return ReturnSuccess();
            }

            throw new DomainException("erro na realização da exclusão de Vendedores");
        }
    }
}
