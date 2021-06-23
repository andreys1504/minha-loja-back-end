using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Core.Authorizations;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.AprovarCadastro;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.RejeitarCadastro;
using MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.VendedoresValidacaoCadastroPendente;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("vendedor")]
    public class VendedorController : ApiControllerBase
    {
        [HttpGet("vendedores-validacao-pendente")]
        [Authorize(Roles = AuthorizationsApplications.AdminLoja.UsuarioMaster)]
        public async Task<IActionResult> VendedoresValidacaoCadastroPendente()
        {
            var requestAppService = new VendedoresValidacaoCadastroPendenteRequest();

            return ReturnApi(HttpStatusCode.OK, await SendRequestService(requestAppService));
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
