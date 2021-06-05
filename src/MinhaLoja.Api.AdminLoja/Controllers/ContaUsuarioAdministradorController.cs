using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.ContaUsuarioAdministrador.CadastrarUsuarioMaster;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.ContaUsuarioAdministrador.CadastrarUsuarioVendedor;
using MinhaLoja.Core.Authorizations;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.CadastroUsuarioMaster;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.CadastroUsuarioVendedor;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.AprovarCadastro;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.RejeitarCadastro;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.ValidarEmail;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("conta-usuario")]
    public class ContaUsuarioAdministradorController : ApiControllerBase
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


        [HttpGet("aprovacao-cadastro-vendedor/{tipo}/{idVendedor}")]
        [Authorize(Roles = AuthorizationsApplications.AdminLoja.UsuarioMaster)]
        public async Task<IActionResult> AprovacaoCadastroVendedor(
            string tipo,
            int idVendedor)
        {
            if (tipo.ToLower() == "aprovar")
            {
                var request = new AprovarCadastroUsuarioVendedorRequest(
                    idVendedor: idVendedor,
                    idUsuario: this.IdUsuario(User)
                );

                return ReturnApi(HttpStatusCode.OK, await SendRequestService(request));
            }
            else if (tipo.ToLower() == "rejeitar")
            {
                var request = new RejeitarCadastroUsuarioVendedorRequest(
                    idVendedor: idVendedor,
                    idUsuario: this.IdUsuario(User)
                );

                return ReturnApi(HttpStatusCode.OK, await SendRequestService(request));
            }
            else
                return ReturnApi(HttpStatusCode.NotFound);
        }
    }
}
