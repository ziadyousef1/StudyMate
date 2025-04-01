using StudyMate.DTOs;

namespace StudyMate.Services.Interfaces;

public interface ICloudStorageService
{
    Task<BlobResponseDto> UploadAsync(string containerName,IFormFile image, string userId);
    Task<BlobResponseDto> DeleteAsync(string containerName,string fileName);
}