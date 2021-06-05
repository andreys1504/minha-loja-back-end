using Microsoft.Azure.ServiceBus;
using MinhaLoja.Core.Infra.ServiceBus;
using MinhaLoja.Core.Messages;
using System.Text;
using System.Threading.Tasks;

namespace MinhaLoja.Infra.ServiceBus
{
    public class ServiceBusManagement : IServiceBusManagement
    {
        //private readonly GlobalSettings _globalSettings;

        //public ServiceBusManagement(GlobalSettings globalSettings)
        //{
        //    _globalSettings = globalSettings;
        //}

        public async Task SendMessageToQueue<TMessage>(
            string connectionStringSend,
            TMessage message,
            string queueName) where TMessage : IMessage
        {
            var client = new QueueClient(
                connectionString: connectionStringSend,
                entityPath: queueName,
                receiveMode: ReceiveMode.PeekLock);

            string messageBody = Helpers.SerializeEntities(message);
            var messageToSend = new Message(Encoding.UTF8.GetBytes(messageBody));
            messageToSend.UserProperties["messageType"] = message.GetType().FullName;

            await client.SendAsync(messageToSend);
            await client.CloseAsync();
        }
    }
}
