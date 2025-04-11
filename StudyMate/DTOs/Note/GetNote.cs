namespace StudyMate.DTOs.Note;

public class GetNote
{
   public string Id { get; set; } 
   public string Title { get; set; }
   public string Content { get; set; }
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}