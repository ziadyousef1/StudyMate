using StudyMate.DTOs;

namespace StudyMate.Services.Interfaces;

public interface IImageService
{
    Task<ServiceResponse> UploadImageAsync(IFormFile image, string userId);
    Task<ServiceResponse> DeleteImageAsync(string UserId);
    
}