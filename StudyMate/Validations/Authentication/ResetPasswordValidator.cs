using FluentValidation;
using StudyMate.DTOs.Authentication;

namespace StudyMate.Validations.Authentication;

public class ResetPasswordValidator:AbstractValidator<ResetPassword>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required")
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required");
    }
    
}