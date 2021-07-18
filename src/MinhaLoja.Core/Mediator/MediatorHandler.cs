using MediatR;
using MinhaLoja.Core.Infra.ServiceBus;
using MinhaLoja.Core.Messages;
using MinhaLoja.Core.Settings;
using System.Threading.Tasks;

namespace MinhaLoja.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IServiceBusManagement _serviceBusManagement;
        private readonly IMediator _mediator;

        public MediatorHandler(
            GlobalSettings globalSettings,
            IServiceBusManagement serviceBusManagement,
            IMediator mediator)
        {
            _globalSettings = globalSettings;
            _serviceBusManagement = serviceBusManagement;
            _mediator = mediator;
        }

        public async Task SendEventToBusAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (_globalSettings.PublishEventsInBus == false)
                await SendEventToHandlersAsync(@event);

            await _serviceBusManagement.SendMessageToQueue(
                connectionStringSend: _globalSettings.ServiceBus.EventQueue.ConnectionStringSend,
                message: @event,
                queueName: _globalSettings.ServiceBus.EventQueue.QueueName);
        }

        public async Task SendEventToHandlersAsync(object @event)
        {
            await _mediator.Publish(@event);
        }

        public async Task<object> SendRequestToHandlerAsync(object request)
        {
            return await _mediator.Send(request);
        }
    }
}
