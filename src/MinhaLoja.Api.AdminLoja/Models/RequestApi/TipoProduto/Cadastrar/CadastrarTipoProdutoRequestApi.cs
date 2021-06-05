using MinhaLoja.Api.AdminLoja.Models.RequestApi.TipoProduto.Cadastrar.RequestApi;
using System.Collections.Generic;

namespace MinhaLoja.Api.AdminLoja.Models.RequestApi.TipoProduto.Cadastrar
{
    public class CadastrarTipoProdutoRequestApi
    {
        public string NomeTipoProduto { get; set; }
        public int? IdTipoProdutoSuperior { get; set; }
        public IList<CaracteristicaTipoProduto> CaracteristicasTipoProduto { get; set; }
    }
}
