using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Api.AdminLoja.Models.RequestApi.TipoProduto.Cadastrar;
using MinhaLoja.Core.Authorizations;
using MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.Cadastro;
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
        [Route("cadastrar")]
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
    }
}
