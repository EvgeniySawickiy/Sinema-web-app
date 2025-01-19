using MediatR;

namespace MovieService.Application.UseCases.Halls.Commands
{
    public class CreateHallCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public int TotalSeats { get; set; }
        public int NumberOfRows { get; set; }
        public List<int> SeatsPerRow { get; set; }

        public CreateHallCommand(string name, int totalSeats, int numberOfRows, List<int> seatsPerRow)
        {
            Name = name;
            TotalSeats = totalSeats;
            NumberOfRows = numberOfRows;
            SeatsPerRow = seatsPerRow;

            if (SeatsPerRow.Count != NumberOfRows)
            {
                throw new ArgumentException("SeatsPerRow count must match the NumberOfRows.");
            }
        }
    }
}
