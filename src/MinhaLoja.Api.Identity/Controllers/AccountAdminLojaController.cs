using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Api.Identity.Models.RequestApi.Account.Authenticate;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Autenticacao;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MinhaLoja.Api.Identity.Controllers
{
    [ApiController]
    [Route("account-admin-loja")]
    public class AccountAdminLojaController : ApiControllerBase
    {
        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult<string>> Authenticate(
            [FromBody] AuthenticateRequestApi requestApi,
            [FromServices] IIdentityService identityService)
        {
            var request = new AutenticacaoUsuarioAdministradorRequest(
                username: requestApi.Username,
                senha: requestApi.Password);

            var response = (IResponseAppService<AutenticacaoUsuarioAdministradorDataResponse>)
                await SendRequestService(request);

            if (!response.Success)
                return Ok(response);

            object token = identityService.GenerateToken(
                requestScheme: ControllerContext.HttpContext.Request.Scheme,
                requestHost: ControllerContext.HttpContext.Request.Host.Value,
                userId: response.Data.IdUsuario.ToString(),
                sellerId: response.Data.IdVendedor.HasValue ? response.Data.IdVendedor.ToString() : null,
                username: response.Data.Username,
                userData: JsonConvert.SerializeObject(response.Data),
                permissions: response.Data.Permissions,
                roles: true);

            var retorno = new
            {
                Success = true,
                response.Notifications,
                Data = new
                {
                    usuario = response.Data,
                    token
                }
            };

            return Ok(retorno);
        }
    }
}
