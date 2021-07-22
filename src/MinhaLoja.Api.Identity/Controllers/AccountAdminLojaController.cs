using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MinhaLoja.Api.Identity.Models.RequestApi.Account.Authenticate;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Settings;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Autenticacao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.Identity.Controllers
{
    [ApiController]
    [Route("account-admin-loja")]
    public class AccountAdminLojaController : ApiControllerBase
    {
        private readonly ITokenManagementService _tokenService;
        private readonly IClaimService _claimService;

        public AccountAdminLojaController(
            ITokenManagementService tokenService,
            IClaimService claimService)
        {
            _tokenService = tokenService;
            _claimService = claimService;
        }


        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate(
            [FromBody] AuthenticateRequestApi requestApi)
        {
            var request = new AutenticacaoUsuarioAdministradorRequest(
                username: requestApi.Username,
                senha: requestApi.Password);

            var response = (IResponseAppService<AutenticacaoUsuarioAdministradorDataResponse>)
                await SendRequestService(request);

            if (response.Success == false)
            {
                return StatusCode(400, response.Notifications[0]);
            }

            var responseAction = GenerateToken(
                userId: response.Data.IdUsuario,
                sellerId: response.Data.IdVendedor,
                username: response.Data.Username,
                userData: response.Data,
                permissions: response.Data.Permissoes);

            return Ok(responseAction);
        }


        [HttpGet]
        [Route("refresh-token")]
        public IActionResult RefreshToken()
        {
            KeyValuePair<string, StringValues> bearerToken = Request.Headers.FirstOrDefault(header => header.Key == "Authorization");
            if (bearerToken.Equals(default(KeyValuePair<string, StringValues>)))
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            string token = bearerToken.Value.First().Replace("Bearer", "").Trim();

            var usuario = JsonConvert
                            .DeserializeObject<AutenticacaoUsuarioAdministradorDataResponse>(_tokenService.CurrentUser(token));

            var responseAction = GenerateToken(
                userId: usuario.IdUsuario,
                sellerId: usuario.IdVendedor,
                username: usuario.Username,
                userData: usuario,
                permissions: usuario.Permissoes);


            return Ok(responseAction);
        }


        private object GenerateToken(
            Guid userId,
            int? sellerId,
            string username,
            object userData,
            IList<string> permissions)
        {
            object token = _tokenService.GenerateToken(
                requestScheme: ControllerContext.HttpContext.Request.Scheme,
                requestHost: ControllerContext.HttpContext.Request.Host.Value,
                userId: userId.ToString(),
                sellerId: sellerId.HasValue ? sellerId.ToString() : null,
                username: username,
                userData: JsonConvert.SerializeObject(userData),
                permissions: permissions,
                roles: true);

            return new
            {
                usuario = userData,
                token
            };
        }
    }
}
