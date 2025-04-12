namespace StudyMate.DTOs
{
    public record LoginResponse
        (   string Id= null!,
            bool IsSuccess= false,
            string Token= null!,
            string Message= null!,
            string RefreshToken=null!,
            string Role=null !
        );
}
