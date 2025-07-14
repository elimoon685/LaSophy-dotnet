using System.Text.Json;
using Azure.Messaging.ServiceBus;
using NotificationsApi.Service;
using SharedContract.Event;

namespace NotificationsApi.Listener
{
    public class CreateUserEventListener:  BackgroundService
    {

        private readonly ServiceBusProcessor _registerProcessor;
        private readonly ServiceBusProcessor _resetProcessor;
        private readonly ISendNotificationService _sendNotificationService;
        private readonly IConfiguration _config;
        private readonly ServiceBusClient _client;
        private readonly ILogger<CreateUserEventListener> _logger;

        public CreateUserEventListener( ISendNotificationService sendEmailService, IConfiguration config, ServiceBusClient client, ILogger<CreateUserEventListener> logger)
        {
            _sendNotificationService = sendEmailService;
            _config = config;
            _client = client;
            _logger = logger;

            
            _registerProcessor = _client.CreateProcessor(_config["AzureServiceBus:QueueName:UserCreated"], new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            });
            _resetProcessor = _client.CreateProcessor(_config["AzureServiceBus:QueueName:EmailVerify"], new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            });

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _registerProcessor.ProcessMessageAsync += HandleRegisterMessageAsync;
            _registerProcessor.ProcessErrorAsync += HandleErrorAsync;

            _resetProcessor.ProcessMessageAsync += HandleResetMessageAsync;
            _resetProcessor.ProcessErrorAsync += HandleErrorAsync;


            await _registerProcessor.StartProcessingAsync(stoppingToken);
            await _resetProcessor.StartProcessingAsync(stoppingToken);
        }

        private async Task HandleRegisterMessageAsync(ProcessMessageEventArgs args)
       {
            try
            {

                var json = args.Message.Body.ToString();
                var userEvent = JsonSerializer.Deserialize<CreateUserEvent>(json);

                if (userEvent != null)
                {

                    await _sendNotificationService.SendWelcomeEmailAsync(userEvent.UserEmail, userEvent.UserName, userEvent.Role);
                }

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private async Task HandleResetMessageAsync(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            var passwordResetEvent = JsonSerializer.Deserialize<PasswordResetTokenGeneratedEvent>(body);

            await _sendNotificationService.SendEmailVerifyEamilAsync(passwordResetEvent.Email, passwordResetEvent.UserName, passwordResetEvent.Token);

            await args.CompleteMessageAsync(args.Message);
        }

        private Task HandleErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception,
        "Message processing failed. Queue: {Queue}, Namespace: {Namespace}, Source: {Source}",
        args.EntityPath,
        args.FullyQualifiedNamespace,
        args.ErrorSource);
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_registerProcessor != null)
            {
                await _registerProcessor.StopProcessingAsync(cancellationToken);
                await _registerProcessor.DisposeAsync();
            }

            if (_resetProcessor != null)
            {
                await _resetProcessor.StopProcessingAsync(cancellationToken);
                await _resetProcessor.DisposeAsync();
            }
            await base.StopAsync(cancellationToken);
        }
    }

}