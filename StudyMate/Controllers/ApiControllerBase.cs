using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace StudyMate.Controllers;

public static class ApiControllerBase
{
 public static string GetUserId(this ControllerBase controller)
 {
     return controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
 }
}