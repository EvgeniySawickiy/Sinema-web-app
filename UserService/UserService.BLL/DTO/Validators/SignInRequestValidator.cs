using FluentValidation;
using UserService.BLL.DTO.Request;

namespace UserService.BLL.DTO.Validators
{
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator()
        {
            RuleFor(r => r.Login)
              .NotEmpty().WithMessage("Login is required")
              .Length(3, 50).WithMessage("The login length must be from 3 to 50 characters")
              .Matches(@"^[A-Za-z0-9_]*$").WithMessage("Login must contain only letters, numbers or underline");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(8, 50).WithMessage("The password length must be from 8 to 50 characters");
        }
    }
}
