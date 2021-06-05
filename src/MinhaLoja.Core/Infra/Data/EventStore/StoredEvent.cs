using MinhaLoja.Core.Messages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace MinhaLoja.Core.Infra.Data.EventStore
{
    public abstract class StoredEvent
    {
        public StoredEvent(
            IEvent @event,
            string user)
        {
            Id = @event.EventId;
            Data = Helpers.SerializeEntities(@event);
            User = user;
            EventType = @event.EventType;
            Timestamp = @event.Timestamp;
        }

        [JsonIgnore]
        [BsonId]
        public ObjectId _id { get; private set; }

        public Guid Id { get; private set; }
        public string Data { get; private set; }
        public string User { get; private set; }
        public string EventType { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}
