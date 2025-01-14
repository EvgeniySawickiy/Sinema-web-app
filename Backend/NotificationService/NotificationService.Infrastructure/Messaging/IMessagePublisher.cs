namespace NotificationService.Infrastructure.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string exchange, string routingKey, T message);
    }
}
