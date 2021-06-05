using MediatR;
using MinhaLoja.Core.Authorizations;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Settings;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.ValidacaoEmail;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Queries;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MensagensUsuario = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Autenticacao
{
    public class AutenticacaoUsuarioAdministradorAppService : AppService<AutenticacaoUsuarioAdministradorDataResponse>,
        IRequestHandler<AutenticacaoUsuarioAdministradorRequest, IResponseService<AutenticacaoUsuarioAdministradorDataResponse>>
    {
        private readonly IUsuarioAdministradorRepository _usuarioAdministradorRepository;
        private readonly GlobalSettings _globalSettings;

        public AutenticacaoUsuarioAdministradorAppService(
            IUsuarioAdministradorRepository usuarioAdministradorRepository,
            GlobalSettings globalSettings,
            DependenciesAppService dependenciesAppService) 
            : base(dependenciesAppService)
        {
            _usuarioAdministradorRepository = usuarioAdministradorRepository;
            _globalSettings = globalSettings;
        }

        public async Task<IResponseService<AutenticacaoUsuarioAdministradorDataResponse>> Handle(
            AutenticacaoUsuarioAdministradorRequest request,
            CancellationToken cancellationToken)
        {
            if (request.Validate() is false)
                return ReturnNotifications(request.Notifications);


            Entities.UsuarioAdministrador usuario =
                _usuarioAdministradorRepository
                    .GetEntity(asNoTracking: false)
                    .Include(usuario => usuario.Vendedor)
                    .FirstOrDefault(UsuarioAdministradorQueries.Autenticar(
                        globalSettings: _globalSettings,
                        username: request.Username,
                        senha: request.Senha)
                    );

            if (usuario == null)
                return ReturnNotification(request.Username, MensagensUsuario.UsuarioAdministrador_Autenticacao_UsernameSenhaMensagemGenerica);


            int? idVendedor = null;
            if (usuario.UsuarioMaster is false)
            {
                if (usuario.Vendedor.EmailUsuarioValidado() is false)
                {
                    string novoCodigo = usuario.Vendedor.GerarNovoCodigoValidacaoEmail();
                    if (usuario.IsValid is false)
                        return ReturnNotifications(usuario.Notifications);

                    if (await CommitAsync())
                    {
                        await PublishEventAsync(
                            @event: new GeradoNovoCodigoValidacaoEmailVendedorEvent(
                                idUsuarioAdministrador: usuario.Id,
                                idVendedor: usuario.Vendedor.Id,
                                emailVendedor: usuario.Vendedor.Email,
                                codigoValidacaoEmail: novoCodigo
                            ),
                                aggregateRoot: usuario
                        );
                    }

                    return ReturnNotification(nameof(usuario.Vendedor.EmailValidado), MensagensUsuario.Vendedor_ValidacaoEmail_Pendente);
                }

                if (usuario.Vendedor.CadastroUsuarioAprovacaoPendente())
                    return ReturnNotification(nameof(usuario.Vendedor.CadastroAprovado), MensagensUsuario.Vendedor_AprovacaoCadastro_CadastroAprovacaoPendente);

                if (usuario.Vendedor.CadastroUsuarioAprovado() is false)
                    return ReturnNotification(nameof(usuario.Vendedor.CadastroAprovado), MensagensUsuario.Vendedor_AprovacaoCadastro_CadastroNaoAprovado);

                idVendedor = usuario.Vendedor.Id;
            }


            return ReturnData(new AutenticacaoUsuarioAdministradorDataResponse
            {
                IdUsuario = usuario.Id2,
                IdVendedor = idVendedor,
                Nome = usuario.Nome,
                Username = usuario.Username,
                Permissions = new List<string>
                {
                    usuario.UsuarioMaster
                    ? AuthorizationsApplications.AdminLoja.UsuarioMaster
                    : AuthorizationsApplications.AdminLoja.UsuarioVendedor
                }
            });
        }
    }
}
