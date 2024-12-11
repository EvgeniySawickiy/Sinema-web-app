using AutoMapper;
using BookingService.Application.DTO;
using BookingService.Core.Entities;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries.Handlers
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingResponseDTO>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<BookingResponseDTO> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<BookingResponseDTO>(await _bookingRepository.GetByIdAsync(request.BookingId));
        }
    }
}
