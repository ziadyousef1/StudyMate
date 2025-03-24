using Microsoft.AspNetCore.Identity;

namespace StudyMate.Models;

public class User:IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ProfilePicture { get; set; }
    public int Points { get; set; }
    
    public virtual List<Notification> Notifications { get; set; } = new();
    
}