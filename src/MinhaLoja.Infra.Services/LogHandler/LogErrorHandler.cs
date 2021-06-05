using Azure.Storage.Queues;
using MinhaLoja.Core.Infra.Services.LogHandler;
using MinhaLoja.Core.Infra.Services.LogHandler.Models;
using MinhaLoja.Core.Settings;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MinhaLoja.Infra.Services.LogHandler
{
    public class LogErrorHandler : ILogErrorHandler
    {
        private readonly GlobalSettings _globalSettings;

        public LogErrorHandler(GlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public async Task SendAsync(ErrorApplicationModel error)
        {
            if (_globalSettings.SendLogErrorToStorage is false)
                return;

            var queueClient = new QueueClient(
                connectionString: _globalSettings.Storage.ConnectionString,
                queueName: _globalSettings.ServiceBus.ErrorQueueName);

            await queueClient.CreateIfNotExistsAsync();

            if (await queueClient.ExistsAsync())
            {
                string messageSerialized = JsonConvert.SerializeObject(error);
                byte[] messageTextBytes = Encoding.UTF8.GetBytes(messageSerialized);
                await queueClient.SendMessageAsync(Convert.ToBase64String(messageTextBytes));
            }
        }
    }
}
