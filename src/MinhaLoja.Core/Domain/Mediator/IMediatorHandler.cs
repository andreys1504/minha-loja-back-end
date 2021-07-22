using MinhaLoja.Core.Messages;
using System.Threading.Tasks;

namespace MinhaLoja.Core.Domain.Mediator
{
    public interface IMediatorHandler
    {
        Task SendDomainEventToBusAsync<TEvent>(TEvent @event) where TEvent : IEvent;

        Task SendDomainEventToHandlersAsync(object @event);

        Task<object> SendRequestToHandlerAsync(object request);
    }
}
