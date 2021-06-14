using Microsoft.EntityFrameworkCore;
using MinhaLoja.Core.Domain.Entities.AggregateBase;
using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using MinhaLoja.Core.Domain.Repositories;
using MinhaLoja.Infra.Data.DataSources;
using MinhaLoja.Infra.Data.DataSources.DatabaseMain;
using System.Linq;
using System.Threading.Tasks;

namespace MinhaLoja.Infra.Data.Repositories.RepositoryBase
{
    public abstract partial class RepositoryBase<TAggregateRoot> : IRepositoryBase<TAggregateRoot> 
        where TAggregateRoot : AggregateRoot
    {
        private readonly DependenciesRepositories _dependenciesRepositories;

        public RepositoryBase(DependenciesRepositories dependenciesRepositories)
        {
            _dependenciesRepositories = dependenciesRepositories;
        }

        public IQueryable<TAggregateRoot> GetEntity(bool asNoTracking = true)
        {
            var aggregateRoot = _dependenciesRepositories.MinhaLojaContext.Set<TAggregateRoot>();

            if (asNoTracking)
                return aggregateRoot.AsNoTracking();

            return aggregateRoot;
        }

        public async Task AddEntityAsync(TAggregateRoot entity)
        {
            await _dependenciesRepositories.MinhaLojaContext.Set<TAggregateRoot>().AddAsync(entity);
        }

        public void DeleteEntity(TAggregateRoot entity)
        {
            _dependenciesRepositories.MinhaLojaContext.Set<TAggregateRoot>().Remove(entity);
        }

        public async Task<bool> DeleteEntityByIdAsync(int idEntity)
        {
            TAggregateRoot aggregateRoot = await _dependenciesRepositories
                .MinhaLojaContext.Set<TAggregateRoot>()
                .FirstOrDefaultAsync(entity => entity.Id == idEntity);

            if (aggregateRoot != null)
            {
                DeleteEntity(aggregateRoot);
                return true;
            }

            return false;
        }

        public IQueryable<TAggregate> GetEntityAggregate<TAggregate>() 
            where TAggregate : Aggregate<TAggregateRoot>
        {
            return _dependenciesRepositories.MinhaLojaContext.Set<TAggregate>();
        }
    }
}
