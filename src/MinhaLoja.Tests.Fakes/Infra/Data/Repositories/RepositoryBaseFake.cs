using MinhaLoja.Core.Domain.Entities.AggregateBase;
using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using MinhaLoja.Core.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhaLoja.Tests.Fakes.Infra.Data.Repositories
{
    public class RepositoryBaseFake<TAggregateRoot> : IRepositoryBase<TAggregateRoot> 
        where TAggregateRoot : AggregateRoot
    {
        private readonly IList<TAggregateRoot> _entities;
        protected readonly IQueryable<IAggregate> _entitiesAggregate;

        public RepositoryBaseFake(
            IList<TAggregateRoot> entities,
            IQueryable<IAggregate> entitiesAggregate = null)
        {
            _entities = entities;
            _entitiesAggregate = entitiesAggregate;
        }

        public Task AddEntityAsync(TAggregateRoot entity)
        {
            if (_entities != null)
                _entities.Add(entity);

            return Task.CompletedTask;
        }

        public void DeleteEntity(TAggregateRoot entity)
        {
            if(_entities != null && _entities.Count > 0)
            {
                var entitySelected = _entities.FirstOrDefault(entity => entity.Id == entity.Id);
                if (entitySelected != null)
                    _entities.Remove(entity);
            }
        }

        public IQueryable<TAggregateRoot> GetEntity(bool asNoTracking = true)
        {
            if (_entities == null)
                return null;

            return _entities.AsQueryable();
        }

        public IQueryable<TAggregate> GetEntityAggregate<TAggregate>() where TAggregate : Aggregate<TAggregateRoot>
        {
            if (_entitiesAggregate == null)
                return null;

            return (IQueryable<TAggregate>)_entitiesAggregate;
        }
    }
}
