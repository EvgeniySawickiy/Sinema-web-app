namespace BookingService.Application.DTO
{
    public class SeatDTO
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public bool IsReserved { get; set; }
    }
}
