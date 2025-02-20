using BookingService.DataAccess.Persistence.Interfaces;
using BookingServiceGrpc;
using Grpc.Core;

namespace BookingService.DataAccess.ExternalServices;

public class BookingStatisticsService : BookingServiceGrpc.BookingStatisticsService.BookingStatisticsServiceBase
{
    private readonly IBookingRepository _bookingRepository;

    public BookingStatisticsService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public override async Task<BookingStatisticsResponse> GetBookingStatistics(BookingStatisticsRequest request, ServerCallContext context)
    {
        var totalBookings = await _bookingRepository.GetTotalCountAsync();
        var totalRevenue = await _bookingRepository.GetTotalRevenueAsync();

        var bookingsByDay = await _bookingRepository.GetBookingsByDayAsync();

        var response = new BookingStatisticsResponse
        {
            TotalBookings = totalBookings,
            TotalRevenue = (double)totalRevenue,
        };

        response.BookingsByDay.AddRange(bookingsByDay.Select(b => new BookingByDay
        {
            Date = b.Date,
            Count = b.Count,
        }));

        return response;
    }
}