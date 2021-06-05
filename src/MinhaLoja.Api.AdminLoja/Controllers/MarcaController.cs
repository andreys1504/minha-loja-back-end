using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.Marca.Cadastrar;
using MinhaLoja.Domain.Catalogo.ApplicationServices.Marca.Cadastro;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("marca")]
    public class MarcaController : ApiControllerBase
    {
        [HttpPost]
        [Route("cadastrar")]
        public async Task<IActionResult> Cadastrar(
            [FromBody] CadastrarMarcaRequestApi requestApi)
        {
            var request = new CadastroMarcaRequest(
                nomeMarca: requestApi.NomeMarca,
                idUsuario: IdUsuario(User)
            );

            return ReturnApi(HttpStatusCode.Created, await SendRequestService(request));
        }
    }
}
