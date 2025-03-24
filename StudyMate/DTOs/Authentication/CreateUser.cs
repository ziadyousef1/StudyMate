namespace EcommerceApp.Application.DTOs.Identity
{
    public class CreateUser : BaseModel
    {
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
    }
}
