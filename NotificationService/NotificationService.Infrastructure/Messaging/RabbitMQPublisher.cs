using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace NotificationService.Infrastructure.Messaging
{
    public class RabbitMQPublisher : IMessagePublisher
    {
        private readonly IConnection _connection;

        public RabbitMQPublisher(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
        }

        public async Task PublishAsync<T>(string exchange, string routingKey, T message)
        {
            using var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            await Task.CompletedTask;
        }
    }
}
