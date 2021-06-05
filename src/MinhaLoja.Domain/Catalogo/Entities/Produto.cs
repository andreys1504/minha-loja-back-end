using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.Entities
{
    public class Produto : AggregateRoot
    {
        protected Produto() : base(null)
        {
        }

        public Produto(
            string nome,
            decimal valor,
            int idMarca,
            string descricao,
            string idExterno,
            IList<(int idCaracteristicaTipoProduto, string descricao)> caracteristicasProduto,
            int idTipoProduto,
            int? idVendedor,
            Guid idUsuario) : base(idUsuario)
        {
            Nome = nome.TrimString();
            ValorAtual = valor;
            MarcaId = idMarca;
            Descricao = descricao.TrimString();
            IdExterno = idExterno.TrimString();
            TipoProdutoId = idTipoProduto;
            VendedorId = idVendedor;

            if (caracteristicasProduto?.Count > 0)
                foreach (var caracteristica in caracteristicasProduto)
                    CaracteristicasProduto.Add(new ProdutoCaracteristica(
                            descricao: caracteristica.descricao,
                            idProduto: this.Id,
                            idTipoProdutoCaracteristicas: caracteristica.idCaracteristicaTipoProduto,
                            idUsuario: idUsuario)
                    );
        }

        public string Nome { get; private set; }
        public decimal? ValorAnterior { get; private set; }
        public decimal ValorAtual { get; private set; }
        public int MarcaId { get; private set; }
        public string Descricao { get; private set; }
        public string IdExterno { get; private set; }
        public int TipoProdutoId { get; private set; }
        public int? VendedorId { get; private set; }

        public Marca Marca { get; private set; }
        public TipoProduto TipoProduto { get; private set; }
        public Vendedor Vendedor { get; private set; }
        public ICollection<ProdutoCaracteristica> CaracteristicasProduto { get; private set; } = new List<ProdutoCaracteristica>();
    }
}
