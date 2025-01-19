using MovieService.Core.Entities;

namespace MovieService.Application.Services;

public class HallService
{
    private readonly SeatLayoutSerializer _serializer;

    public HallService()
    {
        _serializer = new SeatLayoutSerializer();
    }

    public string GenerateSeatLayoutJson(int numberOfRows, List<int> seatsPerRow)
    {
        var seatLayout = new SeatLayout();

        for (int i = 0; i < numberOfRows; i++)
        {
            seatLayout.Rows.Add(new Row
            {
                RowNumber = i + 1,
                Seats = Enumerable.Range(1, seatsPerRow[i]).ToList(),
            });
        }

        return _serializer.Serialize(seatLayout);
    }
}