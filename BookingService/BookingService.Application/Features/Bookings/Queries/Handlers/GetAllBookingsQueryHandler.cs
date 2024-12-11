using AutoMapper;
using BookingService.Application.DTO;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries.Handlers
{
    public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, IEnumerable<BookingResponseDTO>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetAllBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingResponseDTO>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<BookingResponseDTO>>(await _bookingRepository.GetAllAsync());
        }
    }
}
