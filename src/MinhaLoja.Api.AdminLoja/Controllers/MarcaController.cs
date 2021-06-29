using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MinhaLoja.Api.AdminLoja.Configurations;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.Marca.Cadastrar;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Domain.Catalogo.ApplicationServices.Marca.Cadastro;
using MinhaLoja.Domain.Catalogo.ApplicationServices.Marca.MarcasCadastroProduto;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("marca")]
    public class MarcaController : ApiControllerBase
    {
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Cadastrar(
            [FromBody] CadastrarMarcaRequestApi requestApi)
        {
            var requestAppService = new CadastroMarcaRequest(
                nomeMarca: requestApi.NomeMarca,
                idUsuario: IdUsuario(User)
            );

            return ReturnApi(HttpStatusCode.Created, await SendRequestService(requestAppService));
        }


        [HttpGet]
        [Route("marcas-cadastro-produto")]
        public async Task<IActionResult> MarcasCadastroProduto(
            [FromServices] IDistributedCache cache)
        {
            string keyCache = KeysCacheApi.Marcas.ToString();
            string response = cache.GetString(keyCache);
            if (response == null)
            {
                var requestAppService = new MarcasCadastroProdutoRequest();
                var responseAppService = (IResponseAppService<IList<MarcasCadastroProdutoDataResponse>>)
                    await SendRequestService(requestAppService);

                response = SerializeReturnApi(responseAppService);

                cache.SetString(keyCache, response, GetOptionsSaveCache());
            }

            return ReturnApi(HttpStatusCode.OK, response, true);
        }
    }
}
