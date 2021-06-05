using MinhaLoja.Core.Domain.Entities.AggregateBase;
using System;

namespace MinhaLoja.Domain.Catalogo.Entities
{
    public class ProdutoCaracteristica : Aggregate<Produto>
    {
        protected ProdutoCaracteristica() : base(null)
        {
        }

        public ProdutoCaracteristica(
            string descricao,
            int idProduto,
            int idTipoProdutoCaracteristicas,
            Guid idUsuario) : base(idUsuario)
        {
            Descricao = descricao.TrimString();
            ProdutoId = idProduto;
            TipoProdutoCaracteristicasId = idTipoProdutoCaracteristicas;
        }

        public string Descricao { get; private set; }
        public int ProdutoId { get; private set; }
        public int TipoProdutoCaracteristicasId { get; private set; }

        public Produto Produto { get; private set; }
        public TipoProdutoCaracteristica CaracteristicasTipoProduto { get; private set; }
    }
}
