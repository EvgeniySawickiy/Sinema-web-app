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
    private readonly string _exchangeName;
    private readonly string _queueName;
    private readonly string _bookingCreatedRoutingKey;
    private readonly string _bookingCancelledRoutingKey;
    private IConnection _connection;
    private IModel _channel;

    public BookingEventConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _exchangeName = configuration["RabbitMQ:ExchangeName"] ?? "BookingExchange";
        _queueName = configuration["RabbitMQ:QueueName"] ?? "NotificationQueue";
        _bookingCreatedRoutingKey = configuration["RabbitMQ:RoutingKeys:BookingCreated"] ?? "booking.created";
        _bookingCancelledRoutingKey = configuration["RabbitMQ:RoutingKeys:BookingCancelled"] ?? "booking.cancelled";

        var factory = new ConnectionFactory
        {
            Uri = new Uri(configuration.GetConnectionString("RabbitMQ")),
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        try
        {
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Topic);
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false,
                arguments: null);
            _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: $"{_exchangeName}.*");
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

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
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
            if (routingKey == _bookingCreatedRoutingKey)
            {
                var bookingCreatedEvent = JsonSerializer.Deserialize<BookingCreatedEvent>(body);
                if (bookingCreatedEvent != null)
                {
                    await notificationService.HandleBookingCreatedAsync(bookingCreatedEvent);

                    notificationService.ScheduleEmailReminders(bookingCreatedEvent);
                }
            }
            else if (routingKey == _bookingCancelledRoutingKey)
            {
                var bookingCancelledEvent = JsonSerializer.Deserialize<BookingCancelledEvent>(body);
                if (bookingCancelledEvent != null)
                {
                    await notificationService.HandleBookingCancelledAsync(bookingCancelledEvent);

                    notificationService.DeleteScheduleEmailReminders(bookingCancelledEvent);
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