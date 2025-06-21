using System.Text;
using Azure.Messaging.ServiceBus;
using CommentsApi;
using CommentsApi.Exceptions;
using CommentsApi.Repository;
using CommentsApi.Services.BookCommentsServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//set cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });

});
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BookCommentsDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LaSophyCommentsDb")));


//Dependency injection

builder.Services.AddScoped<IBookCommentsRepository, BookCommentsRepository>();
builder.Services.AddScoped<IBookCommentsService, BookCommentsServices>();
builder.Services.AddSingleton<GlobalExceptionsHandler>();

//service bus
//Auth 
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]))
    };
});
/*
builder.Services.AddSingleton<ServiceBusClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config["AzureServiceBus:ConnectionString"];
    return new ServiceBusClient(connStr);
});
*/
/*
builder.Services.AddSingleton<ServiceBusSender>(provider =>
{
    var client = provider.GetRequiredService<ServiceBusClient>();
    var queueName = provider.GetRequiredService<IConfiguration>()["AzureServiceBus:QueueName"];
    return client.CreateSender(queueName);
});
*/
//autoMapper

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
app.UseExceptionHandler(

    errorApp => errorApp.Run(async context =>
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<GlobalExceptionsHandler>();
        await exceptionHandler.HandleException(context);
    })
   );
// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
