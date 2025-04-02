namespace StudyMate.DTOs.Authentication
{
    public class RegisterUser : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ConfirmPassword { get; set; }
  
    }
}
