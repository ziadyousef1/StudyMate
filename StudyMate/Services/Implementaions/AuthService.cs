using System.Security.Cryptography;
using AutoMapper;
using EmailService;
using FluentValidation;
using StudyMate.DTOs;
using StudyMate.DTOs.Authentication;
using StudyMate.Models;
using StudyMate.Repositories.Interfaces;
using StudyMate.Services.Interfaces;
using StudyMate.Services.Interfaces.Authentication;
using StudyMate.Validations;
using StudyMate.Validations.Authentication;

namespace StudyMate.Services.Implementaions
{
    public class AuthService(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IRoleRepository roleRepository,
        IAppLogger<AuthService> logger,
        IMapper mapper,
        IValidator<RegisterUser> createUserValidator,
        IValidator<LoginUser> loginUserValidator,
        IValidator<ResetPassword> resetPasswordValidator,
        IValidator<ConfirmEmail> confirmEmailValidator,
        IValidator<ForgotPassword> forgotPasswordValidator,
        IValldationService validationService,
        IEmailSender emailSender,
        IVerificationCodeRepository verificationCodeRepository) :IAuthService
    {

        public async Task<ServiceResponse> CreateUser(RegisterUser user)
        {
            var  validationResult = await validationService.ValidateAsync(user, createUserValidator);
            if(!validationResult.IsSuccess)
            {
                return validationResult;
            }
            var mappedUser = mapper.Map<AppUser>(user);
            mappedUser.UserName = user.Email;
            mappedUser.PasswordHash = user.Password;
            var result = await userRepository.CreateUser(mappedUser);
            if(!result.Succeeded)
            {
                return new ServiceResponse { Message = "User creation failed", IsSuccess = false };
            }

            var _user = await userRepository.GetUserByEmail(user.Email);
            var assignedResult = await roleRepository.AddUserToRole(_user, "User");
            
            
            if(!assignedResult)
            {
               int removeUserResult = await userRepository.RemoveUserByEmail(_user.Email);
                if(removeUserResult <= 0)
                {
                    logger.LogError( new Exception($"User with email {_user.Email} could not be removed"), "User Could not be assigned role" );
                    return new ServiceResponse{Message ="Error occured on creating account", IsSuccess = false};
                }
            }
            var code = GenerateRandomNumber();
            await verificationCodeRepository.StoreVerificationCode(_user.Id, code);
            await emailSender.CreateAccountConfirmationEmail(_user,code.ToString());
            return new ServiceResponse{Message = "Account created successfully", IsSuccess = true};


        }

        public async Task<LoginResponse> Login(LoginUser user)
        {
            var validationResult = await validationService.ValidateAsync(user, loginUserValidator);
            if(!validationResult.IsSuccess)
            {
                return new LoginResponse( Message: validationResult.Message);
                
            }
            var mappedModel = mapper.Map<AppUser>(user);
            mappedModel.PasswordHash = user.Password;
            
            var loginResult = await userRepository.LoginUser(mappedModel);
            if(!loginResult.IsSuccess)
            {
                return loginResult;
            }
            var _user = await userRepository.GetUserByEmail(user.Email);
            var claims = await userRepository.GetUserClaims(_user.Email);
            var refreshToken = tokenRepository.GenerateRefreshToken();
            var jwtToken = tokenRepository.GenerateToken(claims);
            int saveTokenResult = await tokenRepository.AddRefreshToken(_user.Email, refreshToken);
            var role = await roleRepository.GetUserRole(_user.Email);
            if(saveTokenResult <= 0)
            {
                logger.LogError(new Exception($"Refresh token could not be saved for user {_user.Email}"), "Refresh token could not be saved");
                return new LoginResponse(Message: "Error occured on login");
            }
            return new LoginResponse(IsSuccess: true, Token: jwtToken, RefreshToken: refreshToken,Role:role);
          
        }

        public async Task<ServiceResponse> ConfirmEmail(ConfirmEmail confirmEmail)
        {
            var validationResult = await validationService.ValidateAsync(confirmEmail, confirmEmailValidator);
            if(!validationResult.IsSuccess)
            {
                return validationResult;
            }
            var result = await verificationCodeRepository.VerifyCode(confirmEmail.Code);
            if(result)
            {
                var user = await userRepository.GetUserByEmail(confirmEmail.Email);
                user.EmailConfirmed = true;
                await userRepository.UpdateUser(user);
            }
            return !result ? new ServiceResponse(Message: "Invalid verification code", IsSuccess: false) 
                           : new ServiceResponse(Message: "Email confirmed successfully", IsSuccess: true);
        }

        public async Task<ServiceResponse> ForgotPassword(ForgotPassword forgotPassword)
        {
            var validationResult = await validationService.ValidateAsync(forgotPassword, forgotPasswordValidator);
            if(!validationResult.IsSuccess)
            {
                return validationResult;
            }
            var user = await userRepository.GetUserByEmail(forgotPassword.Email);
            if(user is null)
            {
                return new ServiceResponse(Message: "User not found", IsSuccess: false);
            }
            var code = GenerateRandomNumber();
            await verificationCodeRepository.StoreVerificationCode(user.Id, code);
             await emailSender.CreatePasswordConfirmationEmail(user, code.ToString());
            return new ServiceResponse(Message: "Password reset code sent to email", IsSuccess: true);
        } 

        public async Task<ServiceResponse> ResetPassword(ResetPassword resetPassword)
        {
           
            var user = await userRepository.GetUserByEmail(resetPassword.Email);
            if(user is null)
            {
                return new ServiceResponse(Message: "Invalid email address", IsSuccess: false);
            }
            var result = await verificationCodeRepository.VerifyCode(resetPassword.Code);
            if(!result)
            {
                return new ServiceResponse(Message: "Invalid verification code", IsSuccess: false);
            }
            
            await verificationCodeRepository.DeleteVerificationCode(resetPassword.Code);
            var  resetResult = await userRepository.ResetPassword(user, resetPassword);
            return resetResult ? new ServiceResponse(Message: "Password reset successfully", IsSuccess: true) 
                                : new ServiceResponse(Message: "Password reset failed", IsSuccess: false);
  
        }


        public async Task<ServiceResponse> VerifyEmail(string email)
        {
            var user =await userRepository.GetUserByEmail(email);
            if(user is null)
               return new ServiceResponse(Message: "User not found", IsSuccess: false);
            
            var code = GenerateRandomNumber();
            await verificationCodeRepository.StoreVerificationCode(user.Id, code);
            await emailSender.CreateAccountConfirmationEmail(user, code.ToString());
            return new ServiceResponse(Message: "Verification code sent to email", IsSuccess: true);

        }

        public async Task<LoginResponse> RefreshToken(string RefreshToken)
        {
          bool validateTokenResult = await tokenRepository.ValidateRefreshToken(RefreshToken);
            if(!validateTokenResult)
            {
                return new LoginResponse(Message: "Invalid refresh token");
            }
            var userId = await tokenRepository.GetUserIdByRefreshToken(RefreshToken); 
            var claims = await userRepository.GetUserClaims(userId);
            var newJwtToken = tokenRepository.GenerateToken(claims);
            var NewRefreshToken = tokenRepository.GenerateRefreshToken();
            int saveTokenResult = await tokenRepository.AddRefreshToken(userId, NewRefreshToken);
          
            return new LoginResponse(IsSuccess: true, Token: newJwtToken, RefreshToken: NewRefreshToken);
            
          
        }
        public async Task<ServiceResponse> VerifyOtp(int code)
        {
            var result = await verificationCodeRepository.VerifyCode( code);
            return result ? new ServiceResponse(Message: "Code verified successfully", IsSuccess: true) 
                          : new ServiceResponse(Message: "Invalid code", IsSuccess: false);
           
        }
        private int GenerateRandomNumber()
        {
            var bytes = new byte[4];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);                                    
            return Math.Abs(BitConverter.ToInt32(bytes, 0)) % 900000 +10000;
        }
    }
}
