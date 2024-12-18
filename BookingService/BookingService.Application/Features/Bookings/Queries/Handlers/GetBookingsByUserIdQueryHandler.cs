using AutoMapper;
using BookingService.Application.DTO;

using BookingService.Core.Exceptions;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries.Handlers
{
    public class GetBookingsByUserIdQueryHandler : IRequestHandler<GetBookingsByUserIdQuery, IEnumerable<BookingResponseDTO>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingsByUserIdQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingResponseDTO>> Handle(GetBookingsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var booking = _mapper.Map<List<BookingResponseDTO>>(await _bookingRepository.GetByUserIdAsync(request.UserId));
            if (booking.Count == 0)
            {
                throw new EmptyUserBookingListException(request.UserId);
            }

            return booking;
        }
    }
}
