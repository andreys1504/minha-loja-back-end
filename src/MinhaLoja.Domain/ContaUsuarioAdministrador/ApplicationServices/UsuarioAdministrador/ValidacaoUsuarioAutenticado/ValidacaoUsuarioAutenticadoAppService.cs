using MediatR;
using MinhaLoja.Core.Authorizations;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Queries;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Validacao
{
    public class ValidacaoUsuarioAutenticadoAppService : AppService<ValidacaoUsuarioAutenticadoDataResponse>,
        IRequestHandler<ValidacaoUsuarioAutenticadoRequest, IResponseAppService<ValidacaoUsuarioAutenticadoDataResponse>>
    {
        private readonly IUsuarioAdministradorRepository _usuarioAdministradorRepository;

        public ValidacaoUsuarioAutenticadoAppService(
            IUsuarioAdministradorRepository usuarioAdministradorRepository,
            DependenciesAppService dependenciesAppService) 
            : base(dependenciesAppService)
        {
            _usuarioAdministradorRepository = usuarioAdministradorRepository;
        }

        public Task<IResponseAppService<ValidacaoUsuarioAutenticadoDataResponse>> 
            Handle(ValidacaoUsuarioAutenticadoRequest request, CancellationToken cancellationToken)
        {
            if(request.Validate() == false)
            {
                return Task.FromResult(ReturnNotifications(request.Notifications));
            }

            var usuario =
                _usuarioAdministradorRepository
                    .GetEntity(asNoTracking: false)
                    .Include(usuario => usuario.Vendedor)
                    .Where(UsuarioAdministradorQueries.ValidarUsuarioAutenticado(
                        idUsuario: request.IdUsuarioEnvioRequest,
                        username: request.Username))
                    .Select(usuario => new
                    {
                        usuario.Id,
                        usuario.Id2,
                        usuario.UsuarioMaster,
                        usuario.Vendedor,
                        usuario.Nome,
                        usuario.Username
                    })
                    .FirstOrDefault();

            if (usuario == null)
            {
                throw new DomainException("Usuário não encontrado");
            }

            return Task.FromResult(ReturnData(new ValidacaoUsuarioAutenticadoDataResponse
            {
                IdUsuario = usuario.Id2,
                IdVendedor = usuario.UsuarioMaster == false ? usuario.Vendedor.Id : null,
                Nome = usuario.Nome,
                Username = usuario.Username,
                Permissoes = new List<string>
                {
                    usuario.UsuarioMaster
                    ? AuthorizationsApplications.AdminLoja.UsuarioMaster
                    : AuthorizationsApplications.AdminLoja.UsuarioVendedor
                }
            }));
        }
    }
}
