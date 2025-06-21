using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserApi;
using UserApi.Models;
using UserApi.Service.UserRoleService;
using UserApi.Repository.UserRepository;
using UserApi.Repository.UserRoleRepository;
using UserApi.Service.UserService;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserApi.Exceptions;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using UserApi.DTO;
using Azure.Messaging.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//---------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
    });

});
//add massTranit+RabbitMQ
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });


});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//------------------set dbcontext

builder.Services.AddDbContext<LaSophyDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LaSophyDb"))
);
builder.Services.AddAutoMapper(typeof(Program));
//-----------------------------------identity

builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<LaSophyDbContext>().AddDefaultTokenProviders();
//dependency injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddSingleton<GlobalExceptionHandler>();

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

builder.Services.AddControllers()
        .AddFluentValidation(
        fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestDto>()
        )
        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//------------------------set cors
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("scope", "admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireClaim("scope", "user"));
});

var app = builder.Build();
app.UseExceptionHandler(

    errorApp => errorApp.Run(async context =>
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<GlobalExceptionHandler>();
        await exceptionHandler.HandleException(context);
    })
   );

using (var scope = app.Services.CreateScope())
{
    var roleService = scope.ServiceProvider.GetRequiredService<IUserRoleService>();

    await roleService.CreateRoleAsync("User");
    await roleService.CreateRoleAsync("Admin");
}

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
app.UseAuthentication(); //
app.UseAuthorization();
app.MapControllers();

app.Run();
