namespace StudyMate.DTOs;

public class BlobDto
{
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public Stream? FileStream { get; set; }
    public string? ContentType { get; set; }
    
}