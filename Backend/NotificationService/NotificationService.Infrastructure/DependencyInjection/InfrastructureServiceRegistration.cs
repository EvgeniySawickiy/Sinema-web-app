﻿using Microsoft.Extensions.Options;
using NotificationService.Core.Entities;
using NotificationService.Infrastructure.Protos;

namespace NotificationService.Infrastructure.DependencyInjection
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using NotificationService.Application.Interfaces;
    using NotificationService.Core.Interfaces;
    using NotificationService.Infrastructure.Messaging;
    using NotificationService.Infrastructure.Persistence.Repositories;
    using NotificationService.Infrastructure.Services;
    using RabbitMQ.Client;
    using StackExchange.Redis;

    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var connectionString = configuration.GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(configuration["MongoDb:DatabaseName"]);
            });

            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(configuration.GetConnectionString("RabbitMQ")),
            };

            services.AddSignalR();
            services.AddSingleton<IConnectionFactory>(connectionFactory);
            services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
            services.AddHostedService<BookingEventConsumer>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IPushNotificationService, PushNotificationService>();

            services.AddScoped<IUserService, UserServiceGrpcClient>();

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionString);
            });

            return services;
        }
    }
}
