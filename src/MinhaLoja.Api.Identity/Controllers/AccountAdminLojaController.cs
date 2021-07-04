using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Api.Identity.Models;
using MinhaLoja.Api.Identity.Models.RequestApi.Account.Authenticate;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Settings;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Autenticacao;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Validacao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MinhaLoja.Api.Identity.Controllers
{
    [ApiController]
    [Route("account-admin-loja")]
    public class AccountAdminLojaController : ApiControllerBase
    {
        private readonly ITokenService _tokenService;

        public AccountAdminLojaController(ITokenService tokenService)
        {
            _tokenService = tokenService;
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
        public async Task<IActionResult> RefreshToken(
            [FromServices] GlobalSettings globalSettings)
        {
            string tokenJwt = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(tokenJwt))
            {
                return StatusCode(401);
            }

            string urlValidarToken = $"{globalSettings.UrlApiAdminLoja}/{globalSettings.UrlValidateTokenAdminLoja}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenJwt.Replace("Bearer", "").Replace("bearer", "").TrimString());
            HttpResponseMessage respostaValidacao = await httpClient.GetAsync(urlValidarToken);
            if (respostaValidacao.IsSuccessStatusCode == false)
            {
                return StatusCode(401);
            }


            IEnumerable<Claim> claims = _tokenService.GetClaims(tokenJwt: tokenJwt);

            if (claims == null)
            {
                throw new DomainException("Erro RefreshToken");
            }

            var user = JsonConvert.DeserializeObject<UsuarioProfissionalAutenticado>(
                    _tokenService.GetUserData(new ClaimsPrincipal(new ClaimsIdentity(claims)))
            );

            var request = new ValidacaoUsuarioAutenticadoRequest(
                idUsuario: user.IdUsuario,
                username: user.Username);

            var response = (IResponseAppService<ValidacaoUsuarioAutenticadoDataResponse>)
                await SendRequestService(request);

            if (response.Success == false)
            {
                return StatusCode(400, response.Notifications[0]);
            }

            var responseAction = GenerateToken(
                userId: user.IdUsuario,
                sellerId: response.Data.IdVendedor,
                username: response.Data.Username,
                userData: response.Data,
                permissions: response.Data.Permissoes);

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
