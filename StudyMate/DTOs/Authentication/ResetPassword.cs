namespace StudyMate.DTOs.Authentication;

public class ResetPassword
{
    public string Email { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
    public int Code { get; set; }
    
}