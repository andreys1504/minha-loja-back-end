using MinhaLoja.Core.Messages;
using System;

namespace MinhaLoja.Core.Infra.Data.EventStore
{
    public class StoredDomainEvent : StoredEvent
    {
        public StoredDomainEvent(
            IEvent @event,
            Guid dataId,
            string user) : base(@event, user)
        {
            DataId = dataId;
        }

        public Guid DataId { get; private set; }
    }
}
