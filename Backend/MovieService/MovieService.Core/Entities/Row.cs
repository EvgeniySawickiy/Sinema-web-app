namespace MovieService.Core.Entities;

public class Row
{
    public int RowNumber { get; set; }
    public List<int> Seats { get; set; } = new List<int>();
}