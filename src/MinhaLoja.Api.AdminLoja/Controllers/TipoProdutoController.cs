using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MinhaLoja.Api.AdminLoja.Configurations;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.TipoProduto.Cadastrar;
using MinhaLoja.Core.Authorizations;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.Cadastro;
using MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.TiposProdutos;
using MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.TiposProdutosCadastroProduto;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("tipo-produto")]
    [Authorize(Roles = AuthorizationsApplications.AdminLoja.UsuarioMaster)]
    public class TipoProdutoController : ApiControllerBase
    {
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Cadastrar(
            [FromBody] CadastrarTipoProdutoRequestApi requestApi)
        {
            var caracteristicasTipoProduto
                = new List<(string nome, string observacao)>();
            if (requestApi.CaracteristicasTipoProduto?.Count > 0)
            {
                foreach (var caracteristica in requestApi.CaracteristicasTipoProduto)
                    caracteristicasTipoProduto
                        .Add((caracteristica.Nome, caracteristica.Observacao));
            }

            var request = new CadastroTipoProdutoRequest(
                nomeTipoProduto: requestApi.NomeTipoProduto,
                idTipoProdutoSuperior: requestApi.IdTipoProdutoSuperior,
                caracteristicasTipoProduto: caracteristicasTipoProduto,
                idUsuario: IdUsuario(User)
            );

            return ReturnApi(HttpStatusCode.Created, await SendRequestService(request));
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> TiposProdutos(
            [FromQuery] int? idTipoProdutoSuperior,
            [FromServices] IDistributedCache cache)
        {
            string keyCache = KeysCacheApi.TiposProdutos.ToString();
            string response = cache.GetString(keyCache);
            if (response == null)
            {
                var requestAppService = new TiposProdutosRequest(idTipoProdutoSuperior);
                var responseAppService = (IResponseAppService<IList<TiposProdutosDataResponse>>)
                    await SendRequestService(requestAppService);

                response = SerializeReturnApi(responseAppService);

                cache.SetString(keyCache, response, GetOptionsSaveCache());
            }

            return ReturnApi(HttpStatusCode.OK, response, true);
        }


        [HttpGet]
        [Route("tipos-produtos-cadastro-produto")]
        public async Task<IActionResult> TiposProdutosCadastroProduto(
            [FromQuery] int? idTipoProdutoSuperior,
            [FromServices] IDistributedCache cache)
        {
            string keyCache = KeysCacheApi.TiposProdutosCadastroProduto.ToString();
            string response = cache.GetString(keyCache);
            if (response == null)
            {
                var requestAppService = new TiposProdutosCadastroProdutoRequest(idTipoProdutoSuperior);
                var responseAppService = (IResponseAppService<IList<TiposProdutosCadastroProdutoDataResponse>>)
                    await SendRequestService(requestAppService);

                response = SerializeReturnApi(responseAppService);

                cache.SetString(keyCache, response, GetOptionsSaveCache());
            }

            return ReturnApi(HttpStatusCode.OK, response, true);
        }
    }
}
