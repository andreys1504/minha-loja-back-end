using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using MinhaLoja.Core.Domain.Events;
using System.Threading.Tasks;

namespace MinhaLoja.Core.Domain.ApplicationServices.Service
{
    public abstract partial class AppService<TDataResponse>
    {
        private readonly DependenciesAppService _dependenciesAppService;

        public AppService(DependenciesAppService dependenciesAppService)
        {
            _dependenciesAppService = dependenciesAppService;
        }

        protected async Task<bool> CommitAsync()
        {
            return (await _dependenciesAppService.ApplicationTransaction.CommitAsync()) > 0;
        }

        protected async Task PublishEventAsync(
            DomainEvent @event,
            AggregateRoot aggregateRoot)
        {
            if (@event != null)
            {
                @event.DomainEventFactory(
                    aggregateRootId: aggregateRoot.Id2,
                    userId: aggregateRoot.IdUsuarioUltimaAtualizacao);

                await _dependenciesAppService.MediatorHandler.SendEventToBusAsync(@event);
            }
        }


        private async Task DisposeTransactionAsync()
        {
            if (_dependenciesAppService.ApplicationTransaction != null)
                await _dependenciesAppService.ApplicationTransaction.DisposeAsync();
        }
    }
}
