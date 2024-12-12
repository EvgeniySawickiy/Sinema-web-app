using BookingService.Core.Entities;
using BookingService.Core.Interfaces;
using BookingService.DataAccess.Cache;
using BookingService.DataAccess.ExternalServices;
using BookingService.DataAccess.Messaging;
using BookingService.DataAccess.Persistence;
using BookingService.DataAccess.Persistence.Interfaces;
using BookingService.DataAccess.Persistence.Repositories;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StackExchange.Redis;
using static MovieService.Grpc.MovieService;

namespace BookingService.DataAccess.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<MongoContext>(sp =>
            {
                var connectionString = configuration.GetConnectionString("MongoDB");
                var databaseName = configuration["MongoDB:DatabaseName"];
                return new MongoContext(new MongoDbSettings(connectionString, databaseName));
            });

            var redisConnectionString = configuration.GetSection("Redis:ConnectionString").Value;
            var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
            {
                Uri = new Uri(configuration.GetConnectionString("RabbitMQ")),
                DispatchConsumersAsync = true,
            });

            services.AddSingleton(sp =>
            {
                var connectionFactory = sp.GetRequiredService<IConnectionFactory>();
                var connection = connectionFactory.CreateConnection();

                return connection;
            });

            services.AddSingleton<RabbitMQPublisher>();
            services.AddScoped<IEventPublisher, RabbitMQPublisher>();

            services.AddSingleton(sp =>
            {
                var userServiceAddress = configuration["Grpc:UserService"];
                return GrpcChannel.ForAddress(userServiceAddress);
            });

            services.AddGrpcClient<MovieServiceClient>(o =>
            {
                o.Address = new Uri(configuration["Grpc:MovieService"]);
            });
            services.AddSingleton<MovieServiceGrpcClient>();

            services.AddScoped<ISeatReservationService, SeatReservationService>();

            services.AddScoped<IRepository<Booking>, BookingRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            return services;
        }
    }
}
