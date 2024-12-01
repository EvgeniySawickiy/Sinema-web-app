namespace MovieService.Core.Exceptions
{
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException(string message)
            : base(message)
        {
        }
    }
}
