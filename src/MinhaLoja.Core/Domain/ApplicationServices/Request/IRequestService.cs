using MinhaLoja.Core.Messages;
using System;

namespace MinhaLoja.Core.Domain.ApplicationServices.Request
{
    public interface IRequestService : IMessage
    {
        Guid RequestId { get; }
        DateTime Timestamp { get; }
        string RequestType { get; }
    }
}
