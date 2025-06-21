using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UploadApi.DTO.ApiResponse;
using UploadApi.Exceptions;
using UploadApi.Extension;
using UploadApi.Service.AzureBlobStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//set httpclient

builder.Services.AddHttpClient("BookApiClient", client =>
{
    //client.BaseAddress = new Uri("https://lasophycommentapi-gscdbndbgbfvc2ha.australiasoutheast-01.azurewebsites.net/");
    client.BaseAddress = new Uri("http://localhost:5014/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
//set cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GlobalExceptionsHandler>();
//injection
builder.Services.AddSingleton<IBookUploadService, BookUploadService>();
builder.Services.AddSingleton(cf =>
{
    var config = cf.GetRequiredService<IConfiguration>();
    var connectionString = config["AzureBlobStorage:ConnectionString"];
    return new BlobServiceClient(connectionString);

});


//set authentication
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
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse(); // Prevent default logic

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var error = ApiResponse<string>.Fail("Unauthorized", "401");

            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    };
});
//set authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});
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
