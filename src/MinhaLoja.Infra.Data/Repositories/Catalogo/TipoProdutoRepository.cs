using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Domain.Catalogo.Repositories;
using MinhaLoja.Infra.Data.DataSources;
using MinhaLoja.Infra.Data.Repositories.RepositoryBase;

namespace MinhaLoja.Infra.Data.Repositories.Catalogo
{
    public class TipoProdutoRepository : RepositoryBase<TipoProduto>, ITipoProdutoRepository
    {
        public TipoProdutoRepository(DependenciesRepositories dependenciesRepositories)
            : base(dependenciesRepositories)
        {
        }
    }
}
