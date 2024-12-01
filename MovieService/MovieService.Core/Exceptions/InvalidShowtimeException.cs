namespace MovieService.Core.Exceptions
{
    public class InvalidShowtimeException : Exception
    {
        public InvalidShowtimeException(string message)
            : base(message) { }
    }
}
