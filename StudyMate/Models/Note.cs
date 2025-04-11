using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyMate.Models;

public class Note
{
    public string Id { get; set; } 
    public string Title { get; set; }
    [Column(TypeName = "nvarchar(1000)")]
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }
    public AppUser User { get; set; }
    
    
}