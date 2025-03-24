using FluentValidation;
using StudyMate.DTOs.Authentication;

namespace StudyMate.Validations.Authentication;

public class ForgotPasswordValidator:AbstractValidator<ForgotPassword>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
    }
}