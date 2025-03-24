namespace EcommerceApp.Application.DTOs
{
    public record LoginResponse
        (
            bool IsSuccess= false,
            string Token= null!,
            string Message= null!,
            string RefreshToken=null!,
            string Role=null !
        );
}
