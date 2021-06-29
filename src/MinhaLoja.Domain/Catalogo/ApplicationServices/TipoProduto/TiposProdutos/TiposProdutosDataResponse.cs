namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.TiposProdutos
{
    public class TiposProdutosDataResponse
    {
        public int IdTipoProduto { get; set; }
        public string NomeTipoProduto { get; set; }
        public int? IdTipoProdutoSuperior { get; set; }
    }
}
