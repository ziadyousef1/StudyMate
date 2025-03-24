
using FluentValidation;
using StudyMate.DTOs;

namespace StudyMate.Validations;

public interface IValldationService
{
    Task<ServiceResponse> ValidateAsync<T>(T model,IValidator<T> validator);
}