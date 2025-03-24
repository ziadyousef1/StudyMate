using FluentValidation;
using StudyMate.DTOs.Authentication;

namespace StudyMate.Validations.Authentication;

public class ConfirmEmailValidator:AbstractValidator<ConfirmEmail>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required");
    }
}