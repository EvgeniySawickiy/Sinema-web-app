﻿using BookingService.Core.Entities;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries
{
    public class GetBookingsByUserIdQuery : IRequest<IEnumerable<Booking>>
    {
        public Guid UserId { get; set; }
    }
}
