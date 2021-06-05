using MinhaLoja.Core.Domain.Entities.AggregateBase;
using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using System.Linq;
using System.Threading.Tasks;

namespace MinhaLoja.Core.Domain.Repositories
{
    public interface IRepositoryBase<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        IQueryable<TAggregateRoot> GetEntity(bool asNoTracking = true);

        Task AddEntityAsync(TAggregateRoot entity);

        void DeleteEntity(TAggregateRoot entity);

        IQueryable<TAggregate> GetEntityAggregate<TAggregate>()
            where TAggregate : Aggregate<TAggregateRoot>;
    }
}
