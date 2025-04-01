using StudyMate.DTOs;
using StudyMate.DTOs.Authentication;


namespace  StudyMate.Services.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<ServiceResponse> CreateUser(RegisterUser user);
        Task<LoginResponse> Login(LoginUser user);
        Task<ServiceResponse> ConfirmEmail(ConfirmEmail confirmEmail);
        Task<ServiceResponse> ForgotPassword(ForgotPassword forgotPassword);
        Task<ServiceResponse> ResetPassword(ResetPassword resetPassword);
        Task<ServiceResponse> VerifyOtp(int code);
        Task<ServiceResponse> VerifyEmail(string email);
        Task<LoginResponse> RefreshToken(string token);
    }
}
