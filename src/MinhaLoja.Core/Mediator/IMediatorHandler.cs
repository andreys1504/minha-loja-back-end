using MinhaLoja.Core.Messages;
using System.Threading.Tasks;

namespace MinhaLoja.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task SendEventToBusAsync<TEvent>(TEvent @event) where TEvent : IEvent;

        Task SendEventToHandlersAsync(object @event);

        Task<object> SendRequestServiceToHandlerAsync(object request);
    }
}
