using Microsoft.ApplicationInsights;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinhaLoja.Core.Domain.Events;
using MinhaLoja.Core.Domain.Mediator;
using MinhaLoja.Core.Infra.Data.EventStore;
using MinhaLoja.Core.Infra.Services.LogHandler;
using MinhaLoja.Core.Infra.Services.LogHandler.Models;
using MinhaLoja.Core.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Worker.DomainEventsReceiver
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly GlobalSettings _globalSettings;
        private readonly QueueClient _queueClient;
        private readonly IServiceProvider _serviceProvider;

        public Worker(
            ILogger<Worker> logger,
            GlobalSettings globalSettings,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _globalSettings = globalSettings;

            _queueClient = new QueueClient(
                connectionString: _globalSettings.ServiceBus.EventQueue.ConnectionStringListen,
                entityPath: _globalSettings.ServiceBus.EventQueue.QueueName,
                receiveMode: ReceiveMode.PeekLock);

            _serviceProvider = serviceProvider;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _queueClient.RegisterMessageHandler(
                    async (message, stoppingToken) =>
                    {
                        await ProcessMessagesAsync(message);
                    },
                    new MessageHandlerOptions(ExceptionReceivedHandler)
                    {
                        MaxConcurrentCalls = 1,
                        AutoComplete = false
                    }
                );
            }, cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"ExecuteAsync: {DateTime.Now}");
            }

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _queueClient.CloseAsync();
            _logger.LogInformation($"StopAsync: {DateTime.Now}");
        }


        #region Auxiliares

        private async Task ProcessMessagesAsync(Message message)
        {
            string nomeEvento = message.UserProperties.First(message => message.Key == "messageType").Value.ToString();
            Type domainEventType = Type.GetType($"{nomeEvento}, MinhaLoja.Domain");
            string body = Encoding.UTF8.GetString(message.Body);
            object @event = JsonConvert.DeserializeObject(body, domainEventType);

            _logger.LogInformation($@"
                ##Mensagem recebida## 
                SequenceNumber: {message.SystemProperties.SequenceNumber} 
                Body: {body}
            ");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var eventStoreRepository = scope.ServiceProvider.GetService<IEventStoreRepository>();

                if (@event is DomainEvent domainEvent)
                {
                    if ((await eventStoreRepository.ExistingEventAsync<StoredDomainEvent>(domainEvent.EventId)) == false)
                    {
                        var storedDomainEvent = new StoredDomainEvent(
                            @event: domainEvent,
                            dataId: domainEvent.AggregateRootId,
                            user: domainEvent.UserId.HasValue 
                                ? domainEvent.UserId.Value.ToString()
                                : null);
                        await eventStoreRepository.SaveAsync(storedDomainEvent);

                        var mediatorHandler = scope.ServiceProvider.GetService<IMediatorHandler>();
                        await mediatorHandler.SendDomainEventToHandlersAsync(@event);
                    }
                }
            }

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            ExceptionReceivedContext context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Exception exception = exceptionReceivedEventArgs.Exception;

            _logger.LogInformation($@"
                {exceptionReceivedEventArgs.Exception.Message}.
                Exception context:
                - Endpoint: {context.Endpoint}
                - Entity Path: {context.EntityPath}
                - Executing Action: {context.Action}
            ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var telemetryClient = scope.ServiceProvider.GetService<TelemetryClient>();
                var properties = new Dictionary<string, string>
                {
                    { nameof(GlobalSettings.CurrentApplication), _globalSettings.CurrentApplication }
                };
                telemetryClient.TrackException(
                    exception: exceptionReceivedEventArgs.Exception,
                    properties: properties);

                var errorHandler = scope.ServiceProvider.GetService<ILogErrorHandler>();
                var error = new ErrorApplicationModel
                {
                    Application = _globalSettings.CurrentApplication,
                    ExceptionTitle = exception.Message,
                    Error = exception.StackTrace,
                    Path = nameof(Worker),
                    UserId = null
                };
                await errorHandler.SendAsync(error: error);
            }
        }

        #endregion
    }
}
