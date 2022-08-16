using PhoneVerification.Models;
using PhoneVerification.Repositories;
using PhoneVerification.Repositories.Interfaces;
using PhoneVerification.Services;
using PhoneVerification.Services.Interfaces;
using SendGrid;
using StackExchange.Redis;
using System.Diagnostics;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var services = builder.Services;

TwilioClient.Init(
  builder.Configuration["AccountSID"],
  builder.Configuration["AuthToken"]
);

services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer> (_ => ConnectionMultiplexer.Connect(builder.Configuration["RedisConnection"]));
services.AddSingleton<ISendGridClient, SendGridClient>(_ => new SendGridClient(builder.Configuration["SendGridApiKey"]));

services.AddScoped<ICodeVerificationService, CodeVerificationService>();
services.AddScoped<IVerificationRepository<string>, RedisVerificationRepository>();
services.AddScoped<IMessageService<Verification, SendMessageOptions>, SMSService>();
services.AddScoped<IMessageService<Verification, SendMessageOptions>, EmailMessageService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
