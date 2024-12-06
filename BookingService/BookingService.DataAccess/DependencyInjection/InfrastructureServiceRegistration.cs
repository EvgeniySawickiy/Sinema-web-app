using BookingService.Core.Entities;
using BookingService.DataAccess.Cache;
using BookingService.DataAccess.Messaging;
using BookingService.DataAccess.Persistence;
using BookingService.DataAccess.Persistence.Interfaces;
using BookingService.DataAccess.Persistence.Repositories;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace BookingService.DataAccess.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // MongoDB
            var mongoSettings = configuration.GetSection("MongoDB").Get<MongoDbSettings>();
            services.AddSingleton(mongoSettings);
            services.AddSingleton<MongoContext>();
            services.AddScoped<IRepository<Booking>, BookingRepository>();

            // Redis
            var redisConnection = configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnection));
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            // RabbitMQ
            services.AddSingleton<IConnectionFactory>(_ =>
                new ConnectionFactory { Uri = new Uri(configuration.GetConnectionString("RabbitMQ")) });
            services.AddScoped<IEventPublisher, RabbitMQPublisher>();

            services.AddSingleton(sp =>
            {
                var movieServiceAddress = configuration["Grpc:MovieServiceAddress"];
                return GrpcChannel.ForAddress(movieServiceAddress);
            });

            services.AddSingleton(sp =>
            {
                var userServiceAddress = configuration["Grpc:UserServiceAddress"];
                return GrpcChannel.ForAddress(userServiceAddress);
            });

            services.AddScoped<IRepository<Booking>, BookingRepository>();
            services.AddScoped<IRepository<Seat>, SeatRepository>();
            return services;
        }
    }
}
