using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Domain.Catalogo.Repositories;
using System.Collections.Generic;

namespace MinhaLoja.Tests.Fakes.Infra.Data.Repositories.Catalogo
{
    public class ProdutoRepositoryFake : RepositoryBaseFake<Produto>, IProdutoRepository
    {
        public ProdutoRepositoryFake(IList<Produto> entities) : base(entities)
        {
        }
    }
}
