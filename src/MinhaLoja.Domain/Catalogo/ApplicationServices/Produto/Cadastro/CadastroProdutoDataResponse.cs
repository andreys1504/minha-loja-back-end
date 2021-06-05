using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.Produto.Cadastro
{
    public class CadastroProdutoDataResponse
    {
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public decimal ValorAtual { get; set; }
        public int IdMarca { get; set; }
        public string DescricaoProduto { get; set; }
        public string IdExterno { get; set; }
        public int IdTipoProduto { get; set; }
        public IEnumerable<string> CaracteristicasProduto { get; set; } = new List<string>();
    }
}
