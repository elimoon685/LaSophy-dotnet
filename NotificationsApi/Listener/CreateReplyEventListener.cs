using System.Text.Json;
using Azure.Messaging.ServiceBus;
using NotificationsApi.Service;
using SharedContract.Event;

namespace NotificationsApi.Listener
{
    public class CreateReplyEventListener:BackgroundService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly ISendNotificationService _sendNotificationService;
        private readonly IConfiguration _config;
        private readonly ServiceBusClient _client;

        public CreateReplyEventListener(ServiceBusProcessor processor, ISendNotificationService sendNotificationService, IConfiguration config, ServiceBusClient client)
        {
            _processor = processor;
            _sendNotificationService = sendNotificationService;
            _config = config;
            _client = client;

            var queueName = _config["AzureServiceBus:QueueName:ReplyPosted"];
            _processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += HandleMessageAsync;
            _processor.ProcessErrorAsync += HandleErrorAsync;

            await _processor.StartProcessingAsync(stoppingToken);
        }

        private async Task HandleMessageAsync(ProcessMessageEventArgs args)
        {
            try
            {

                var json = args.Message.Body.ToString();
                var newReplyEvent = JsonSerializer.Deserialize<CreateNewReplyEvent>(json);
                /*
                if ( newReplyEvent!= null)
                {

                    await _sendNotificationService.SendWelcomeEmailAsync(userEvent.UserEmail, userEvent.UserName, userEvent.Role);
                }
                */
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private Task HandleErrorAsync(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();
        }


    }
}
