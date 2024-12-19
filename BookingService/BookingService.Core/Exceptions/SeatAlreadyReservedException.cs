namespace BookingService.Core.Exceptions
{
    public class SeatAlreadyReservedException : Exception
    {
        public SeatAlreadyReservedException(int row, int number)
            : base($"Seat at row {row}, number {number} is already reserved.")
        {
        }
    }
}
