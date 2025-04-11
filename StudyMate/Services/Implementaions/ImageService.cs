using Microsoft.Extensions.Options;
using StudyMate.DTOs;
using StudyMate.Repositories.Interfaces;
using StudyMate.Services.Interfaces;
using StudyMate.Settings;

namespace StudyMate.Services.Implementaions;

public class ImageService : IImageService
{
    private readonly IUserRepository _userRepository;
    private readonly ICloudStorageService _cloudStorageService;
    private readonly CloudStorageSettings _cloudSettings;

    public ImageService(IUserRepository userRepository,
        ICloudStorageService cloudStorageService,
        IOptions<CloudStorageSettings> azureSettings)
    {
        _userRepository = userRepository;
        _cloudStorageService = cloudStorageService;
        _cloudSettings = azureSettings.Value;
    }

    public async Task<ServiceResponse> UploadImageAsync(IFormFile image, string userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user is null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }
        string? oldImageUrl = user.ProfilePicture;

        var result = await _cloudStorageService.UploadAsync(_cloudSettings.Containers.Images, image, userId);
        if (result.IsSuccess)
        {
            user.ProfilePicture = result.Blob.FileUrl;
            var updated = await _userRepository.UpdateUser(user);
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                var blobUri = new Uri(oldImageUrl);
                string blobName = ExtractBlobNameFromUrl(blobUri.AbsoluteUri);
                await _cloudStorageService.DeleteAsync(_cloudSettings.Containers.Images, blobName);
            }
            return new ServiceResponse
            {
                IsSuccess = true,
                Message = "Image uploaded successfully"
            };


        }
        return new ServiceResponse(Message:result.Message,IsSuccess:false);
        
    }
    public async Task<ServiceResponse> DeleteImageAsync(string UserId) 
    {
        var user = await _userRepository.GetUserById(UserId);
        if (user is null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }
        var oldImageUrl = user.ProfilePicture;
        if(oldImageUrl is null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = "Image not found"
            };
        }
        var blobUri = new Uri(oldImageUrl);
        string fileName = ExtractBlobNameFromUrl(blobUri.AbsoluteUri);
        var result = await _cloudStorageService.DeleteAsync(_cloudSettings.Containers.Images, fileName);
        if (result.IsSuccess)
        {
            return new ServiceResponse
            {
                IsSuccess = true,
                Message = "Image deleted successfully"
            };
        }
        user.ProfilePicture = null;
        await _userRepository.UpdateUser(user);
        
        return new ServiceResponse(Message : result.Message, IsSuccess: false);
    }
    private string ExtractBlobNameFromUrl(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return Path.GetFileName(uri.AbsolutePath);
        }
    
        return url.Split('/').Last();
    }
}



