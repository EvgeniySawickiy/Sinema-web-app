using FluentValidation;
using MovieService.Application.DTO.Movie;

namespace MovieService.Application.Validators
{
    public class CreateMovieValidator : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must be less than 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than 0.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10.");
        }
    }
}
