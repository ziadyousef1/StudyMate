using AutoMapper;
using FluentValidation;
using StudyMate.DTOs;
using StudyMate.DTOs.Authentication;
using StudyMate.Models;
using StudyMate.Repositories.Interfaces;
using StudyMate.Services.Interfaces;
using StudyMate.Services.Interfaces.Authentication;
using StudyMate.Validations;

namespace StudyMate.Services.Implementaions
{
    public class AuthenticationService(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IRoleRepository roleRepository,
        IAppLogger<AuthenticationService> logger,
        IMapper mapper,
        IValidator<CreateUser> createUserValidator,
        IValidator<LoginUser> loginUserValidator,
        IValldationService validationService) : IAuthenticationService
    {

        public async Task<ServiceResponse> CreateUser(CreateUser user)
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
            if(!result)
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
            if(!loginResult)
            {
                return new LoginResponse(Message: "Invalid login attempt");
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
    }
}
