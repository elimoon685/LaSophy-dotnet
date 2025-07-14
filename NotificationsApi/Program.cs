using Azure.Messaging.ServiceBus;
using MassTransit;
using NotificationsApi.Consumer;
using NotificationsApi.Exceptions;
using NotificationsApi.Listener;
using NotificationsApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// add massTransit
/*
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<UserCreatedEventConsumer>();
    config.AddConsumer<PasswordResetCreatedEventConsumer>();
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("user-created-queue", e =>
        {
            e.ConfigureConsumer<UserCreatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("password-reset-queue", e =>
        {
            e.ConfigureConsumer<PasswordResetCreatedEventConsumer>(context);
        });
    });


});
*/
builder.Services.AddSingleton<ISendNotificationService, SendNotificationService>();
builder.Services.AddSingleton<GlobalExceptionsHandler>();
builder.Services.AddSingleton<ServiceBusClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config["AzureServiceBus:ConnectionString"];
    return new ServiceBusClient(connStr);
});

builder.Services.AddSingleton<ServiceBusSender>(provider =>
{
    var client = provider.GetRequiredService<ServiceBusClient>();
    var queueName = provider.GetRequiredService<IConfiguration>()["AzureServiceBus:QueueName"];
    return client.CreateSender(queueName);
});
builder.Services.AddHostedService<CreateUserEventListener>();
//builder.Services.AddHostedService<CreateReplyEventListener>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseExceptionHandler(

    errorApp => errorApp.Run(async context =>
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<GlobalExceptionsHandler>();
        await exceptionHandler.HandleException(context);
    })
   );
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
