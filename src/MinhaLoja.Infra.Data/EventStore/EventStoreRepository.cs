using MinhaLoja.Core.Infra.Data.EventStore;
using MinhaLoja.Infra.Data.DataSources.DatabaseSecondary;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MinhaLoja.Infra.Data.EventStore
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly MinhaLojaContextSecondaryDatabase _context;

        public EventStoreRepository(MinhaLojaContextSecondaryDatabase context)
        {
            _context = context;
        }

        public async Task SaveAsync<TStoredEvent>(TStoredEvent @event) where TStoredEvent : StoredEvent
        {
            await _context
                .GetCollection<TStoredEvent>(GetNameCollection<TStoredEvent>())
                .InsertOneAsync(@event);
        }

        public async Task<bool> ExistingEventAsync<TStoredEvent>(Guid eventId) where TStoredEvent : StoredEvent
        {
            var filter = Builders<TStoredEvent>.Filter.Where(@evento => evento.Id == eventId);

            return await _context
                    .GetCollection<TStoredEvent>(GetNameCollection<TStoredEvent>())
                    .Find(filter).AnyAsync();
        }


        private string GetNameCollection<TStoredEvent>() where TStoredEvent : StoredEvent
        {
            return typeof(TStoredEvent).Name == "StoredDomainEvent"
                ? "DomainEvent"
                : "Event";
        }
    }
}
