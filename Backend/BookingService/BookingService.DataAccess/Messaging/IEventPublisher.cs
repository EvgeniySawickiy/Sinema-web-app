namespace BookingService.DataAccess.Messaging
{
    public interface IEventPublisher
    {
        void Publish(string exchange, string routingKey, object message);
    }
}
