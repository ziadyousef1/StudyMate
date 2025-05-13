using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using StudyMate.Controllers;
using StudyMate.DTOs.Authentication;
using StudyMate.DTOs;
using StudyMate.Services.Interfaces;
using System.Threading.Tasks;
using StudyMate.Services.Interfaces.Authentication;

public class AuthenticationControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthenticationController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenSuccess()
    {
        var loginUser = new LoginUser { Email = "ZiadYousef321@outlock.com", Password = "Password123!" };
        var response = new LoginResponse { IsSuccess = true, Message = "Login successful" }; 
        _authServiceMock.Setup(s => s.Login(loginUser)).ReturnsAsync(response);

        var result = await _controller.Login(loginUser);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenFailed()
    {
        var loginUser = new LoginUser { Email = "ZiadYousef321@outlock.com", Password = "wrong" };
        var response = new LoginResponse { IsSuccess = false, Message = "Invalid credentials" };
        _authServiceMock.Setup(s => s.Login(loginUser)).ReturnsAsync(response);

        var result = await _controller.Login(loginUser);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(response, badRequest.Value);
    }


    [Fact]
    public async Task Register_ReturnsOk_WhenSuccess()
    {
        var registerUser = new RegisterUser { Email = "ZiadYousef321@outlock.com", Password = "Password123!", ConfirmPassword = "Password13!" };
        var response = new ServiceResponse { IsSuccess = true, Message = "Registration successful" };
        _authServiceMock.Setup(s => s.CreateUser(registerUser)).ReturnsAsync(response);

        var result = await _controller.Register(registerUser);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenFailed()
    {
        var registerUser = new RegisterUser { Email = "ZiadYousef321@outlock.com", Password = "Password123!", ConfirmPassword = "Password123!" };
        var response = new ServiceResponse { IsSuccess = false, Message = "Registration failed" };
        _authServiceMock.Setup(s => s.CreateUser(registerUser)).ReturnsAsync(response);

        var result = await _controller.Register(registerUser);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(response, badRequest.Value);
    }

    [Fact]
    public async Task VerifyEmailOtp_ReturnsOk_WhenSuccess()
    {
        var response = new ServiceResponse { IsSuccess = true, Message = "Verification sent" };
        _authServiceMock.Setup(s => s.VerifyEmail("ZiadYousef321@outlock.com")).ReturnsAsync(response);

        var result = await _controller.VerifyEmailOtp("ZiadYousef321@outlock.com");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task VerifyEmailOtp_ReturnsBadRequest_WhenFailed()
    {
        var response = new ServiceResponse { IsSuccess = false, Message = "Verification failed" };
        _authServiceMock.Setup(s => s.VerifyEmail("test@example.com")).ReturnsAsync(response);

        var result = await _controller.VerifyEmailOtp("test@example.com");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(response, badRequest.Value);
    }
}
