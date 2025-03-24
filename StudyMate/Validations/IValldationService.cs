using EcommerceApp.Application.DTOs;
using FluentValidation;

namespace EcommerceApp.Application.Validations;

public interface IValldationService
{
    Task<ServiceResponse> ValidateAsync<T>(T model,IValidator<T> validator);
}