using System;

namespace MinhaLoja.Core.Messages
{
    public interface IEvent : IMessage
    {
        Guid EventId { get; }
        DateTime Timestamp { get; }
        string EventType { get; }
        Guid? UserId { get; }
    }
}
