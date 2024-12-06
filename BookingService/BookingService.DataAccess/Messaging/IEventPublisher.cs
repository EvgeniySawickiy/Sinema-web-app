namespace BookingService.DataAccess.Messaging
{
    public interface IEventPublisher
    {
        void Publish<T>(string exchange, T message);
    }
}
