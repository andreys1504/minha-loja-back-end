using MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.Cadastro.DataResponse;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.Cadastro
{
    public class CadastroTipoProdutoDataResponse
    {
        public int IdTipoProduto { get; set; }
        public string NomeTipoProduto { get; set; }
        public int? IdTipoProdutoSuperior { get; set; }
        public IList<CaracteristicaTipoProduto> CaracteristicasTipoProduto { get; set; }
            = new List<CaracteristicaTipoProduto>();
    }
}
