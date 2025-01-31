using AutoMapper;
using BookingService.Application.DTO;
using BookingService.DataAccess.Persistence.Interfaces;

using MediatR;

namespace BookingService.Application.Features.Bookings.Queries.Handlers;

public class GetAllBookedSeatsHandler : IRequestHandler<GetAllBookedSeatsQuery, List<SeatDTO>>
{
    private readonly ISeatRepository _seatRepository;
    private readonly IMapper _mapper;

    public GetAllBookedSeatsHandler(ISeatRepository seatRepository, IMapper mapper)
    {
        _seatRepository = seatRepository;
        _mapper = mapper;
    }

    public async Task<List<SeatDTO>> Handle(GetAllBookedSeatsQuery request, CancellationToken cancellationToken)
    {
        var seats = await _seatRepository.FindAsync(s => s.ShowTimeId == request.ShowtimeId && s.IsReserved);
        var seatsDto = _mapper.Map<List<SeatDTO>>(seats);
        return seatsDto;
    }
}