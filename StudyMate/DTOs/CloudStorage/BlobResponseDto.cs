namespace StudyMate.DTOs;

public class BlobResponseDto
{
    public BlobResponseDto()
    {
        Blob = new BlobDto();
    }
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public BlobDto Blob { get; set; }  
}