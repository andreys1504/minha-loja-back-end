using MinhaLoja.Core.Domain.Entities.AggregateBase;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.Entities
{
    public class TipoProdutoCaracteristica : Aggregate<TipoProduto>
    {
        protected TipoProdutoCaracteristica() : base(null)
        {
        }

        public TipoProdutoCaracteristica(
            string nome,
            string observacao,
            int idTipoProduto,
            Guid idUsuario) : base(idUsuario)
        {
            Nome = nome.TrimString();
            Observacao = observacao.TrimString();
            TipoProdutoId = idTipoProduto;
        }

        public string Nome { get; private set; }
        public string Observacao { get; private set; }
        public int TipoProdutoId { get; private set; }

        public TipoProduto TipoProduto { get; private set; }
        public ICollection<ProdutoCaracteristica> CaracteristicasProdutos { get; private set; } = new List<ProdutoCaracteristica>();

        public void VincularTipoProduto(TipoProduto tipoProduto)
        {
            TipoProduto = tipoProduto;
        }
    }
}
