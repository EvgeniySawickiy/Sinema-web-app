using AutoMapper;
using BookingService.Application.DTO;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries.Handlers
{
    public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, PaginatedResult<BookingResponseDTO>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetAllBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<BookingResponseDTO>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                var totalCount = await _bookingRepository.GetTotalCountAsync();
                var bookings = await _bookingRepository.GetPagedAsync(request.PageNumber.Value, request.PageSize.Value);
                var bookingDtos = _mapper.Map<List<BookingResponseDTO>>(bookings);

                return new PaginatedResult<BookingResponseDTO>(
                    bookingDtos,
                    totalCount,
                    request.PageNumber.Value,
                    request.PageSize.Value);
            }
            else
            {

                var bookings = await _bookingRepository.GetAllAsync();
                var bookingDtos = _mapper.Map<List<BookingResponseDTO>>(bookings);

                return new PaginatedResult<BookingResponseDTO>(
                    bookingDtos,
                    bookingDtos.Count,
                    1,
                    bookingDtos.Count);
            }
        }
    }
}
