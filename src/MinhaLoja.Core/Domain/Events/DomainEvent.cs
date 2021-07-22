using MinhaLoja.Core.Messages;
using System;

namespace MinhaLoja.Core.Domain.Events
{
    public abstract class DomainEvent : IEvent
    {
        public Guid EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public Guid AggregateRootId { get; set; }
        public Guid? UserId { get; set; }

        //'DomainEventFactory': devido a problemas de deserialização quando da existência de ctor.
        public void DomainEventFactory(
            Guid aggregateRootId,
            Guid? userId)
        {
            EventId = Guid.NewGuid();
            Timestamp = DateTime.Now;
            EventType = this.GetType().Name;
            AggregateRootId = aggregateRootId;
            UserId = userId;
        }
    }
}
