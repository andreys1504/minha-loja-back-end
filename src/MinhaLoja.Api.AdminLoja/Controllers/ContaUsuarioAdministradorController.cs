using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.ContaUsuarioAdministrador.CadastrarUsuarioVendedor;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.CadastroUsuarioVendedor;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.ValidarEmail;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("conta-usuario")]
    public class ContaUsuarioAdministradorController : ApiControllerBase
    {
        [HttpPost("cadastrar-vendedor")]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarUsuarioVendedor(
            [FromBody] CadastrarUsuarioVendedorRequestApi requestApi)
        {
            var request = new CadastroUsuarioVendedorRequest(
                nome: requestApi.Nome,
                email: requestApi.Email,
                senha: requestApi.Senha,
                cnpj: requestApi.Cnpj);

            return ReturnApi(HttpStatusCode.Created, await SendRequestService(request));
        }


        [HttpGet("validar-email-vendedor/{codigo}")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidarEmail(string codigo)
        {
            var request = new ValidarEmailUsuarioVendedorRequest(
                codigo: codigo);

            return ReturnApi(HttpStatusCode.OK, await SendRequestService(request));
        }


        //TODO: remover
        [HttpGet("token-valido")]
        public IActionResult TokenValido()
        {
            return Ok();
        }
    }
}
