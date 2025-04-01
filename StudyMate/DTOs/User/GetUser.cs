namespace StudyMate.DTOs.Profile;

public class GetUser :UserBase
{
    public string Id { get; set; }
    public string Email { get; set; }
    public  string ProfilePicture { get; set; }

    
}