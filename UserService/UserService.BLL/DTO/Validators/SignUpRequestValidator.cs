using FluentValidation;
using UserService.BLL.DTO.Request;

namespace UserService.BLL.DTO.Validators
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(user => user.Login)
            .NotEmpty().WithMessage("Login is required.")
            .Length(3, 50).WithMessage("Login must be between 3 and 50 characters.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .When(user => !string.IsNullOrEmpty(user.PhoneNumber))
                .WithMessage("Invalid phone number format.");

            RuleFor(user => user.Name)
                .Length(1, 50)
                .When(user => !string.IsNullOrEmpty(user.Name))
                .WithMessage("Name must be between 1 and 50 characters.");

            RuleFor(user => user.Surname)
                .Length(1, 50)
                .When(user => !string.IsNullOrEmpty(user.Surname))
                .WithMessage("Surname must be between 1 and 50 characters.");
        }
    }
}
