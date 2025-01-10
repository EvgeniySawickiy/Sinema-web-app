using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Application.Interfaces;
using NotificationService.Core.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService.Infrastructure.Messaging;

public class BookingEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IModel _channel;

    public BookingEventConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory
        {
            Uri = new Uri(configuration.GetConnectionString("RabbitMQ")),
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        try
        {
            _channel.ExchangeDeclarePassive("BookingExchange");
            _channel.QueueDeclarePassive("NotificationQueue");
            _channel.QueueBind(queue: "NotificationQueue", exchange: "BookingExchange", routingKey: "booking.*");
        }
        catch (RabbitMQ.Client.Exceptions.OperationInterruptedException ex)
        {
            throw new Exception($"Error during RabbitMQ setup: {ex.Message}");
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, args) =>
        {
            await OnEventReceived(sender, args);
        };

        _channel.BasicConsume(queue: "NotificationQueue", autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }

    private async Task OnEventReceived(object sender, BasicDeliverEventArgs args)
    {
        var body = Encoding.UTF8.GetString(args.Body.ToArray());
        var routingKey = args.RoutingKey;

        using var scope = _serviceProvider.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        try
        {
            if (routingKey == "booking.created")
            {
                var bookingCreatedEvent = JsonSerializer.Deserialize<BookingCreatedEvent>(body);
                if (bookingCreatedEvent != null)
                {
                    await notificationService.HandleBookingCreatedAsync(bookingCreatedEvent);
                }
            }
            else if (routingKey == "booking.cancelled")
            {
                var bookingCancelledEvent = JsonSerializer.Deserialize<BookingCancelledEvent>(body);
                if (bookingCancelledEvent != null)
                {
                    await notificationService.HandleBookingCancelledAsync(bookingCancelledEvent);
                }
            }

            _channel.BasicAck(args.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _channel.BasicNack(args.DeliveryTag, false, true);
            throw new Exception($"Error processing message: {ex.Message}");
        }
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}