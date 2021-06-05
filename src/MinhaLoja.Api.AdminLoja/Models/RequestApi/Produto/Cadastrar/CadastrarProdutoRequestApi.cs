using MinhaLoja.Api.AdminLoja.Models.RequestApi.Produto.Cadastrar.RequestApi;
using System.Collections.Generic;

namespace MinhaLoja.Api.AdminLoja.Models.RequestApi.Produto.Cadastrar
{
    public class CadastrarProdutoRequestApi
    {
        public string NomeProduto { get; set; }
        public decimal Valor { get; set; }
        public int IdMarca { get; set; }
        public string DescricaoProduto { get; set; }
        public string IdExterno { get; set; }
        public int IdTipoProduto { get; set; }
        public IList<CaracteristicaProduto> CaracteristicasProduto { get; set; }
    }
}
