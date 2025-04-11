using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyMate.DTOs;
using StudyMate.DTOs.Profile;
using StudyMate.Services.Interfaces;

namespace StudyMate.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController:ControllerBase
{
    private readonly IUserService _userService;
    private readonly IImageService _imageService;

    public UsersController(IUserService userService,IImageService imageService)
    {
        _userService = userService;
        _imageService = imageService;
    }
    [HttpPost]
    
    public async Task<IActionResult> Create(CreateUser createUser)
    {
        if(!ModelState.IsValid)
            return BadRequest(new ServiceResponse(Message:"Invalid model state",IsSuccess:false));
        var user = await _userService.CreateUserAsync(createUser);
        if (user is null)
          return BadRequest();
        
        return CreatedAtAction(nameof(Get), new { userId = user.Id }, user);
    }
    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(string userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user is null)
          return NotFound(new ServiceResponse(Message:"User not found",IsSuccess:false));
        return Ok(user);
    }
    [HttpPut]
    public async Task<IActionResult> Update(UpdateUser updateUser)
    {
        var user = await _userService.UpdateUserAsync(updateUser);
        if (user is null)
          return NotFound(new ServiceResponse(Message:"User not found",IsSuccess:false));
        return Ok(user);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteUser deleteUser)
    {
        var result = await _userService.DeleteUserAsync(deleteUser);
        if (result)
          return NoContent();
        return NotFound(new ServiceResponse(Message:"User not found",IsSuccess:false));
    }
    [HttpPost("{userId}/upload-image")]
    public async Task<IActionResult> UploadImage([FromRoute] string userId, IFormFile image)
    {
        if (this.GetUserId() != userId)
           return Unauthorized();
        var result = await _imageService.UploadImageAsync(image, userId);
        if (result.IsSuccess)
          return Ok(result);
        return BadRequest(result);
    }
    
    [HttpDelete("{userId}/delete-image")]
    public async Task<IActionResult> DeleteImage([FromRoute]string userId)
    {
        if (this.GetUserId() != userId)
              return Unauthorized();
        var result = await _imageService.DeleteImageAsync(userId);
        if (result.IsSuccess)
          return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPut("{userId}/update-points")]
    public async Task<IActionResult> UpdatePoints([FromRoute]string userId, [FromBody]int points)
    {
        var result = await _userService.UpdatePointsAsync(userId, points);
        return Ok(result);
    }
    
    
    
    
    
}