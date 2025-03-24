using StudyMate.DTOs;
using StudyMate.DTOs.Authentication;


namespace  StudyMate.Services.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse> CreateUser(CreateUser user);
        Task<LoginResponse> Login(LoginUser user);
        Task<LoginResponse> RefreshToken(string token);
    }
}
