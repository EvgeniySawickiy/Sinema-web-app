using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using NotificationService.Application.Interfaces;
using NotificationService.Core.Entities;
using NotificationService.Infrastructure.DependencyInjection;
using NotificationService.Infrastructure.Hubs;
using NotificationService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<IEmailService, EmailService>(sp =>
{
    var emailSettings = sp.GetRequiredService<IOptions<EmailSettings>>().Value;
    return new EmailService(emailSettings);
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7188/";
        options.Audience = "notification_service";
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },
        };
    });

var app = builder.Build();

app.MapHub<NotificationHub>("/notificationHub");

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
