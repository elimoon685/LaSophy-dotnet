using System.Text.Json;
using Azure.Core.Serialization;
using Azure.Messaging.ServiceBus;
using CommentsApi.DTO;

namespace CommentsApi
{
    public class BookinfoProcessor : BackgroundService
    {
        private readonly ServiceBusProcessor _serviceBusProcessor;

        public BookinfoProcessor(ServiceBusClient client, IConfiguration configuration)
        {
            var queueName = configuration["AzureBlobStorage:QueueName"];
            _serviceBusProcessor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _serviceBusProcessor.ProcessMessageAsync += MessageHandler;
            _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
            await _serviceBusProcessor.StartProcessingAsync(stoppingToken);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var json = args.Message.Body.ToString();
            var bookInfo = JsonSerializer.Deserialize<BookInfoDto>(json);
            Console.WriteLine();

        }
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Error: {args.Exception.Message}");
            return Task.CompletedTask;

        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serviceBusProcessor.StopProcessingAsync();
            await _serviceBusProcessor.DisposeAsync();
        }
    }
}
