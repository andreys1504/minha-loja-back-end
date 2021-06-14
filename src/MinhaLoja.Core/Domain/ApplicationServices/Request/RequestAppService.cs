using Flunt.Notifications;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace MinhaLoja.Core.Domain.ApplicationServices.Request
{
    public abstract class RequestAppService : Notifiable<Notification>, IRequestAppService
    {
        protected RequestAppService()
        {
            RequestId = Guid.NewGuid();
            Timestamp = DateTime.Now;
            RequestType = GetType().Name;
        }

        [JsonIgnore]
        [BsonId]
        public ObjectId _id { get; set; }
        
        public Guid RequestId { get; set; }
        public DateTime Timestamp { get; set; }
        public string RequestType { get; set; }

        [JsonIgnore]
        public abstract Guid IdUsuario { get; }

        public abstract bool Validate();
    }
}
