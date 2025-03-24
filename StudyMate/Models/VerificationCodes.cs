namespace StudyMate.Models;

public class VerificationCode
{
    public int  Id { get; set; }
    public int Code { get; set; }
    public DateTime ExpirationTime { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
}