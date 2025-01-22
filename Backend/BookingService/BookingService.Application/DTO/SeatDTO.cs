namespace BookingService.Application.DTO
{
    public class SeatDTO
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public Guid HallId { get; set; }
        public Guid ShowTimeId { get; set; }

        public bool IsReserved { get; set; }
    }
}
