using System.Text.Json;
using MovieService.Core.Entities;

namespace MovieService.Application.Services;

public class SeatLayoutSerializer
{
    public string Serialize(SeatLayout seatLayout)
    {
        return JsonSerializer.Serialize(seatLayout, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
    }

    public SeatLayout Deserialize(string seatLayoutJson)
    {
        return JsonSerializer.Deserialize<SeatLayout>(seatLayoutJson);
    }
}