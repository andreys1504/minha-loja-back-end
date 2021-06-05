using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.Entities
{
    public class TipoProduto : AggregateRoot
    {
        protected TipoProduto() : base(null)
        {
        }

        public TipoProduto(
            string nome,
            int? idTipoProdutoSuperior,
            Guid? codigoGrupoTipoProduto,
            int? numeroAtualOrdemHierarquiaGrupo,
            IList<(string nome, string observacao)> caracteristicasTipoProduto,
            Guid idUsuario) : base(idUsuario)
        {
            Nome = nome.TrimString();
            Ativo = true;
            TipoProdutoSuperiorId = idTipoProdutoSuperior;
            this.CodigoGrupoTipoProduto = codigoGrupoTipoProduto ?? Guid.NewGuid();
            NumeroOrdemHierarquiaGrupo = numeroAtualOrdemHierarquiaGrupo == null ? 1 : (numeroAtualOrdemHierarquiaGrupo.Value + 1);

            if (caracteristicasTipoProduto?.Count > 0)
                foreach (var caracteristica in caracteristicasTipoProduto)
                    CaracteristicasTipoProduto.Add(new TipoProdutoCaracteristica(
                        nome: caracteristica.nome,
                        observacao: caracteristica.observacao,
                        idTipoProduto: this.Id,
                        idUsuario: idUsuario)
                    );
        }

        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
        public int? TipoProdutoSuperiorId { get; private set; }
        public Guid CodigoGrupoTipoProduto { get; private set; }
        public int NumeroOrdemHierarquiaGrupo { get; private set; }

        public TipoProduto TipoProdutoSuperior { get; private set; }
        public ICollection<TipoProduto> TiposProdutosInferiores { get; private set; } = new List<TipoProduto>();
        public ICollection<TipoProdutoCaracteristica> CaracteristicasTipoProduto { get; private set; } = new List<TipoProdutoCaracteristica>();
        public ICollection<Produto> Produtos { get; private set; } = new List<Produto>();
    }
}
