using System.ComponentModel.DataAnnotations;

namespace StudyMate.DTOs.Profile;

public class UserBase
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
}