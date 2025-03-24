using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using StudyMate.DTOs.Authentication;
using StudyMate.Services.Interfaces.Authentication;

namespace StudyMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IAuthService authenticationService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
    {
        var result = await authenticationService.Login(loginUser);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] CreateUser createUser)
    {
        var result = await authenticationService.CreateUser(createUser);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("request-email-verification")]
    public async Task<IActionResult> VerifyEmailOtp(string email)
    {
        var result = await authenticationService.VerifyEmail(email);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    
    
    [HttpPost("verify-email-otp")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmail confirmEmail)
    {
        var result = await authenticationService.ConfirmEmail(confirmEmail);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
    {
        var result = await authenticationService.ForgotPassword(forgotPassword);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
    {
        var result = await authenticationService.ResetPassword(resetPassword);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpPost("verify-password-reset-otp")]
    
    public async Task<IActionResult> VerifyResetOtp(int code)
    {
        var result = await authenticationService.VerifyOtp(code);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpPost("refreshToken/{RefreshToken}")]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        var result = await authenticationService.RefreshToken(HttpUtility.UrlDecode(refreshToken));
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    
}