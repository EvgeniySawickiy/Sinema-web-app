namespace BookingService.Core.Exceptions;

public class SeatIsNotReservedException : System.Exception
{
    public SeatIsNotReservedException(int row, int number)
        : base($"Seat at row {row}, number {number} is not reserved.")
    {
    }
}
