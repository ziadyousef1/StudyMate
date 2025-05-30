﻿
using FluentValidation;
using StudyMate.DTOs.Authentication;

namespace StudyMate.Validations.Authentication
{
    public class CreateUserValidator : AbstractValidator<RegisterUser>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");
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
