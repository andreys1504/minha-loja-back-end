using MinhaLoja.Core.Messages;
using System.Threading.Tasks;

namespace MinhaLoja.Core.Infra.ServiceBus
{
    public interface IServiceBusManagement
    {
        Task SendMessageToQueue<TMessage>(
            string connectionStringSend,
            TMessage message,
            string queueName) where TMessage : IMessage;
    }
}
