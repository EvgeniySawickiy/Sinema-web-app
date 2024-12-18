namespace BookingService.Application.DTO;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    private int TotalCount { get; set; }

    private int PageNumber { get; set; }
    private int PageSize { get; set; }
    private int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    private bool HasNextPage => PageNumber < TotalPages;
    private bool HasPreviousPage => PageNumber > 1;

    public PaginatedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}