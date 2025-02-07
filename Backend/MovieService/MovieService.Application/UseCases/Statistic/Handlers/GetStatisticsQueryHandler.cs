using BookingServiceGrpc;
using MediatR;
using MovieService.Application.DTO;
using MovieService.Application.UseCases.Statistic.Queries;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Statistic.Handlers;

public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BookingStatisticsService.BookingStatisticsServiceClient _bookingClient;

    public GetStatisticsQueryHandler(
        IUnitOfWork unitOfWork,
        BookingStatisticsService.BookingStatisticsServiceClient bookingClient)
    {
        _unitOfWork = unitOfWork;
        _bookingClient = bookingClient;
    }

    public async Task<StatisticsDto> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        var totalShowtimes = await _unitOfWork.Showtimes.CountAsync(cancellationToken);
        var totalMovies = await _unitOfWork.Movies.CountAsync(cancellationToken);
        var totalHalls = await _unitOfWork.Halls.CountAsync(cancellationToken);

        var bookingResponse = await _bookingClient.GetBookingStatisticsAsync(new BookingStatisticsRequest());

        var bookingsByDay = bookingResponse.BookingsByDay
            .Select(b => new BookingByDayDto
            {
                Date = b.Date,
                Count = b.Count,
            }).ToList();

        return new StatisticsDto
        {
            TotalShowtimes = totalShowtimes,
            TotalMovies = totalMovies,
            TotalHalls = totalHalls,
            TotalBookings = bookingResponse.TotalBookings,
            TotalRevenue = (decimal)bookingResponse.TotalRevenue,
            BookingsByDay = bookingsByDay,
        };
    }
}