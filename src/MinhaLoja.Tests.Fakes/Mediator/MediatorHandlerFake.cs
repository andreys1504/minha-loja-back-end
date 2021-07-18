using MinhaLoja.Core.Mediator;
using MinhaLoja.Core.Messages;
using System.Threading.Tasks;

namespace MinhaLoja.Tests.Fakes.Mediator
{
    public class MediatorHandlerFake : IMediatorHandler
    {
        public Task SendEventToBusAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return Task.CompletedTask;
        }

        public Task SendEventToHandlersAsync(object @event)
        {
            return Task.CompletedTask;
        }

        public Task<object> SendRequestToHandlerAsync(object request)
        {
            return Task.FromResult<object>("");
        }
    }
}
