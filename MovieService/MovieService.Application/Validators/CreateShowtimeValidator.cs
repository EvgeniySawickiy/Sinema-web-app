using FluentValidation;
using MovieService.Application.DTO.Showtime;

namespace MovieService.Application.Validators
{
    public class CreateShowtimeValidator : AbstractValidator<CreateShowtimeDto>
    {
        public CreateShowtimeValidator()
        {
            RuleFor(x => x.MovieId)
                .NotEmpty().WithMessage("MovieId is required.");

            RuleFor(x => x.StartTime)
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future.");

            RuleFor(x => x.HallId)
                .NotEmpty().WithMessage("HallId is required.");
        }
    }
}
