using System;
using System.Threading.Tasks;

namespace MinhaLoja.Core.Infra.Data.EventStore
{
    public interface IEventStoreRepository
    {
        Task SaveAsync<TStoredEvent>(TStoredEvent @event) where TStoredEvent : StoredEvent;

        Task<bool> ExistingEventAsync<TStoredEvent>(Guid eventId) where TStoredEvent : StoredEvent;
    }
}
