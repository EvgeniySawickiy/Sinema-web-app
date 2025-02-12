using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace BookingService.DataAccess.Messaging
{
    public class RabbitMQPublisher : IEventPublisher
    {
        private readonly IConnection _connection;

        public RabbitMQPublisher(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
        }

        public void Publish(string exchange, string routingKey, object message)
        {
            using var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);

            channel.QueueDeclare(queue: "NotificationQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.QueueBind(queue: "NotificationQueue", exchange: "BookingExchange", routingKey: "booking.*");
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(
                           exchange: exchange,
                           routingKey: routingKey,
                           basicProperties: null,
                           body: body);
        }
    }
}
