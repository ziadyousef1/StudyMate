
using EcommerceApp.Application.DTOs.Identity;
using FluentValidation;

namespace EcommerceApp.Application.Validations.Authentication
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.Password).
                NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.Password)
               .MinimumLength(6).WithMessage("Password must be at least 6 characters")
               .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
               .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
               .Matches("[0-9]").WithMessage("Password must contain at least one number")
               .Matches(@"[^\w]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}
