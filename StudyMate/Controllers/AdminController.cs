using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyMate.Repositories.Interfaces;

namespace StudyMate.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AdminController(IUserRepository userRepository) : ControllerBase
{
 
    [HttpPost("delete-all-users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAllUsers()
    {
       var result =await userRepository.DeleteUserAllUsers();
         if (result)
           return Ok("All users deleted successfully");
         
         return BadRequest("Failed to delete all users");
    }
    [HttpPost("delete-user")]
    [Authorize(Roles = "Admin,User")]

    public async Task<IActionResult> DeleteUser(string email)
    {
        var result = await userRepository.RemoveUserByEmail(email);
        if (result > 0)
           return Ok("User deleted successfully");
        return BadRequest("Failed to delete user");
    }

}