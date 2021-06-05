using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Domain.Catalogo.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MinhaLoja.Tests.Fakes.Infra.Data.Repositories.Catalogo
{
    public class TipoProdutoRepositoryFake : RepositoryBaseFake<TipoProduto>, ITipoProdutoRepository
    {
        public TipoProdutoRepositoryFake(
            IList<TipoProduto> entities,
            IQueryable<TipoProdutoCaracteristica> entitiesAggregate = null) : base(entities, entitiesAggregate)
        {
        }
    }
}
