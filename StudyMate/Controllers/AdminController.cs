using Microsoft.AspNetCore.Mvc;
using StudyMate.Repositories.Interfaces;

namespace StudyMate.Controllers;

public class AdminController(IUserRepository userRepository) : ControllerBase
{
 
    [HttpPost("delete-all-users")]
    public async Task<IActionResult> DeleteAllUsers()
    {
       var result =await userRepository.UpdateUserAllUsers();
         if (result)
           return Ok("All users deleted successfully");
         
         return BadRequest("Failed to delete all users");
    }
}