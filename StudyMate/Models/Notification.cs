namespace StudyMate.Models;

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
    public string UserId { get; set; }
    public AppUser AppUser { get; set; }
    
}