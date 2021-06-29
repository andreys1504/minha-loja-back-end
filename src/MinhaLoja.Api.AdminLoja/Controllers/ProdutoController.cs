using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.Produto.Cadastrar;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.Produto.Cadastrar.RequestApi;
using MinhaLoja.Domain.Catalogo.ApplicationServices.Produto.Cadastro;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("produto")]
    public class ProdutoController : ApiControllerBase
    {
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Cadastrar(
            [FromBody] CadastrarProdutoRequestApi requestApi)
        {
            var caracteristicasProduto
                = new List<(int idCaracteristicaProduto, string descricao)>();
            if (requestApi.CaracteristicasProduto?.Count > 0)
            {
                foreach (CaracteristicaProduto caracteristica in requestApi.CaracteristicasProduto)
                    caracteristicasProduto
                        .Add((caracteristica.IdCaracteristicaProduto, caracteristica.Descricao));
            }


            var request = new CadastroProdutoRequest(
                nomeProduto: requestApi.NomeProduto,
                valor: requestApi.Valor,
                idMarca: requestApi.IdMarca,
                descricaoProduto: requestApi.DescricaoProduto,
                idExterno: requestApi.IdExterno,
                idTipoProduto: requestApi.IdTipoProduto,
                caracteristicaProduto: caracteristicasProduto,
                idVendedor: IdVendedor(User),
                idUsuario: IdUsuario(User)
            );

            return ReturnApi(HttpStatusCode.Created, await SendRequestService(request));
        }
    }
}
