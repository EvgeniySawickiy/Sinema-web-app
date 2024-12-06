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

        public void Publish<T>(string exchange, T message)
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(
                exchange: exchange,
                routingKey: string.Empty,
                basicProperties: null,
                body: body);
        }
    }
}
