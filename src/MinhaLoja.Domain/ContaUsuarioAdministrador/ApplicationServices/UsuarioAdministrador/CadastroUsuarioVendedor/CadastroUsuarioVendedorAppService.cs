using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Core.Settings;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.UsuarioAdministrador.CadastroUsuarioVendedor;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Queries;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.CadastroUsuarioVendedor
{
    public class CadastroUsuarioVendedorAppService : AppService<string>,
        IRequestHandler<CadastroUsuarioVendedorRequest, IResponseAppService<string>>
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IUsuarioAdministradorRepository _usuarioAdministradorRepository;

        public CadastroUsuarioVendedorAppService(
            GlobalSettings globalSettings,
            IUsuarioAdministradorRepository usuarioAdministradorRepository,
            DependenciesAppService dependenciesAppService) 
            : base(dependenciesAppService)
        {
            _globalSettings = globalSettings;
            _usuarioAdministradorRepository = usuarioAdministradorRepository;
        }

        public async Task<IResponseAppService<string>> Handle(
            CadastroUsuarioVendedorRequest request,
            CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
                return ReturnNotifications(request.Notifications);

            bool usuarioExistente =
                _usuarioAdministradorRepository
                    .GetEntity()
                    .Include(usuario => usuario.Vendedor)
                    .Any(UsuarioAdministradorQueries.UsuarioVendedorExistenteSistema(
                        email: request.Email,
                        cnpj: request.Cnpj)
                    );

            if (usuarioExistente)
                return ReturnNotification("Email/CNPJ", MensagensVendedor.Vendedor_Cadastro_NotificacaoUsuarioExistente);

            Entities.UsuarioAdministrador usuarioAdministrador = await CadastrarUsuarioVendedor(request: request);

            if(usuarioAdministrador == null)
                throw new DomainException("erro na realização do cadastro do usuário Vendedor");

            if (usuarioAdministrador.IsValid == false)
                return ReturnNotifications(usuarioAdministrador.Notifications);

            
            return ReturnData(MensagensVendedor.Vendedor_Cadastro_Sucesso);
        }


        private async Task<Entities.UsuarioAdministrador> CadastrarUsuarioVendedor(CadastroUsuarioVendedorRequest request)
        {
            var usuarioAdministrador = new Entities.UsuarioAdministrador(
                nome: request.Nome,
                usernameEmail: request.Email,
                senha: request.Senha,
                globalSettings: _globalSettings);

            usuarioAdministrador.VincularVendedor(new Entities.Vendedor(
                email: request.Email,
                cnpj: request.Cnpj,
                idUsuario: usuarioAdministrador.Id)
            );

            if (usuarioAdministrador.IsValid == false)
                return usuarioAdministrador;

            await _usuarioAdministradorRepository.AddEntityAsync(usuarioAdministrador);

            if (await CommitAsync())
            {
                await PublishEventAsync(
                    @event: new UsuarioVendedorCadastradoEvent(
                        idUsuarioAdministrador: usuarioAdministrador.Id,
                        idVendedor: usuarioAdministrador.Vendedor.Id,
                        nomeVendedor: usuarioAdministrador.Nome,
                        emailVendedor: usuarioAdministrador.Vendedor.Email,
                        cnpj: usuarioAdministrador.Vendedor.Cnpj,
                        codigoValidacaoEmail: usuarioAdministrador.Vendedor.CodigoValidacaoEmail
                    ),
                    aggregateRoot: usuarioAdministrador
                );

                return usuarioAdministrador;
            }

            return null;
        }
    }
}
