using System.ComponentModel.DataAnnotations;

namespace StudyMate.DTOs.Profile;

public class CreateUser:UserBase
{ 
   
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    

}