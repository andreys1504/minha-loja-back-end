using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Core.Settings;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.UsuarioAdministrador.CadastroUsuarioMaster;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Queries;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MensagensUsuario = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.CadastroUsuarioMaster
{
    public class CadastroUsuarioMasterAppService : AppService<bool>,
        IRequestHandler<CadastroUsuarioMasterRequest, IResponseAppService<bool>>
    {
        private readonly IUsuarioAdministradorRepository _usuarioAdministradorRepository;
        private readonly GlobalSettings _globalSettings;

        public CadastroUsuarioMasterAppService(
            IUsuarioAdministradorRepository usuarioAdministradorRepository,
            GlobalSettings globalSettings,
            DependenciesAppService dependenciesAppService) 
            : base(dependenciesAppService)
        {
            _usuarioAdministradorRepository = usuarioAdministradorRepository;
            _globalSettings = globalSettings;
        }

        public async Task<IResponseAppService<bool>> Handle(
            CadastroUsuarioMasterRequest request, 
            CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
                return ReturnNotifications(request.Notifications);


            bool usuarioExistente =
                _usuarioAdministradorRepository
                    .GetEntity()
                    .Any(UsuarioAdministradorQueries.UsuarioExistenteSistema(
                        username: request.Username)
                    );

            if (usuarioExistente)
                return ReturnNotification(nameof(request.Username), MensagensUsuario.UsuarioMaster_Cadastro_UsuarioJaCadastradaSistema);


            var usuarioAdministrador = new Entities.UsuarioAdministrador(
                nome: request.Nome,
                usernameEmail: request.Username,
                senha: request.Senha,
                globalSettings: _globalSettings);

            if (usuarioAdministrador.IsValid == false)
                return ReturnNotifications(usuarioAdministrador.Notifications);

            await _usuarioAdministradorRepository.AddEntityAsync(usuarioAdministrador);

            if (await CommitAsync())
            {
                await PublishEventAsync(
                    @event: new UsuarioMasterCadastradoEvent(
                        idUsuarioAdministrador: usuarioAdministrador.Id,
                        nome: usuarioAdministrador.Nome,
                        username: usuarioAdministrador.Username
                    ),
                    aggregateRoot: usuarioAdministrador
                );

                return ReturnSuccess();
            }

            throw new DomainException("erro na realização do cadastro do Usuário Master");
        }
    }
}
