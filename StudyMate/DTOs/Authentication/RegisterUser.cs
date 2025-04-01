namespace StudyMate.DTOs.Authentication
{
    public class RegisterUser : BaseModel
    {
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
