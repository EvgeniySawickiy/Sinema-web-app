using BookingService.Application.Features.Bookings.Commands;
using FluentValidation;

namespace BookingService.Application.Features.Validators
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ShowtimeId).NotEmpty();
            RuleFor(x => x.Seats).NotEmpty();
            RuleFor(x => x.TotalAmount).GreaterThan(0);
        }
    }
}
