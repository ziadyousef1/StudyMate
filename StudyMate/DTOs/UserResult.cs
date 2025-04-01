namespace StudyMate.DTOs;

public class UserResult
{
    public bool Succeeded { get; set; }
    public string? UserId { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}