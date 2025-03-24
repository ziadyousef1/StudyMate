    using EcommerceApp.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using IAuthenticationService = EcommerceApp.Application.Services.Interfaces.Authentication.IAuthenticationService;

namespace EcommerceApp.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
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