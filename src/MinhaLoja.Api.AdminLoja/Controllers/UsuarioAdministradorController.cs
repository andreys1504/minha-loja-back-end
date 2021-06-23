using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.UsuarioAdministrador.CadastrarUsuarioMaster;
using MinhaLoja.Core.Authorizations;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.CadastroUsuarioMaster;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("usuario-administrador")]
    public class UsuarioAdministradorController : ApiControllerBase
    {
        [HttpPost("cadastrar-usuario-master")]
        //[AllowAnonymous]
        [Authorize(Roles = AuthorizationsApplications.AdminLoja.UsuarioMaster)]
        public async Task<IActionResult> CadastrarUsuarioMaster(
            [FromBody] CadastrarUsuarioMasterRequestApi requestApi)
        {
            var request = new CadastroUsuarioMasterRequest(
                nome: requestApi.Nome,
                username: requestApi.Username,
                senha: requestApi.Senha,
                idUsuario: IdUsuario(User)
            );

            return ReturnApi(HttpStatusCode.Created, await SendRequestService(request));
        }
    }
}
