using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.Entities
{
    public class Marca : AggregateRoot
    {
        protected Marca() : base(null)
        {
        }

        public Marca(
            string nome,
            Guid idUsuario) : base(idUsuario)
        {
            Nome = nome.TrimString();
        }

        public string Nome { get; private set; }

        public ICollection<Produto> Produtos { get; private set; } = new List<Produto>();

        public void Editar(string nome)
        {
            Nome = nome.TrimString();
        }
    }
}
