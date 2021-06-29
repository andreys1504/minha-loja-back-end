namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.TiposProdutosCadastroProduto
{
    public class TiposProdutosCadastroProdutoDataResponse
    {
        public int IdTipoProduto { get; set; }
        public string NomeTipoProduto { get; set; }
        public int? IdTipoProdutoSuperior { get; set; }
    }
}
