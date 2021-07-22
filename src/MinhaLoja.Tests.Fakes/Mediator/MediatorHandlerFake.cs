using MinhaLoja.Core.Domain.Mediator;
using MinhaLoja.Core.Messages;
using System.Threading.Tasks;

namespace MinhaLoja.Tests.Fakes.Mediator
{
    public class MediatorHandlerFake : IMediatorHandler
    {
        public Task SendDomainEventToBusAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return Task.CompletedTask;
        }

        public Task SendDomainEventToHandlersAsync(object @event)
        {
            return Task.CompletedTask;
        }

        public Task<object> SendRequestToHandlerAsync(object request)
        {
            return Task.FromResult<object>("");
        }
    }
}
