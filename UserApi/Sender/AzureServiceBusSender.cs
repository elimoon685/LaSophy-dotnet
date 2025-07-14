using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace UserApi.Sender
{
    public class AzureServiceBusSender
    {
        private readonly ServiceBusClient _serviceBusClient;

        public AzureServiceBusSender(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }
        public async Task SendMessageAsync<T>(string queueName,T obj)
        {
            ServiceBusSender sender = _serviceBusClient.CreateSender(queueName);
            string json=JsonSerializer.Serialize(obj);
            var message = new ServiceBusMessage(json)
            {
                ContentType = "application/json",
                Subject = typeof(T).Name // Or hardcode like "UserCreated", if you want
            };
            try
            {
                await sender.SendMessageAsync(message);
            }
            catch (Exception err)
            {
                throw new Exception();
            }

        }
    }
}
