
using FluentValidation;
using StudyMate.DTOs;
using StudyMate.Validations;

namespace StudyMate.Validations;

public class ValidationService : IValldationService
{
    public async Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            var errorsString = string.Join(", ", errors);
            return  new ServiceResponse{Message =errorsString, IsSuccess = false};
            
            
        }
        return new ServiceResponse{IsSuccess = true};
    }
}