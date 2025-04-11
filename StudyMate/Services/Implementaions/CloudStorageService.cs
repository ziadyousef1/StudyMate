using System.Reflection.Metadata;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;
using StudyMate.DTOs;
using StudyMate.Services.Interfaces;
using StudyMate.Settings;

namespace StudyMate.Services.Implementaions;

public class CloudStorageService: ICloudStorageService
{
    private readonly CloudStorageSettings _cloudSettings;
    private readonly BlobServiceClient _blobServiceClient;

    public CloudStorageService(IOptions<CloudStorageSettings> azureSettings)
    {
        _cloudSettings = azureSettings.Value;
        _blobServiceClient = new BlobServiceClient(_cloudSettings.ConnectionString);

    }
    

    public async Task<BlobResponseDto> UploadAsync(string containerName,IFormFile file, string userId)
    {

        var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobName = $"{userId}-{file.FileName}";
        var blobClient = blobContainer.GetBlobClient(blobName);
            
        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = file.ContentType
        };

        await blobClient.UploadAsync(file.OpenReadStream(), new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
            
        return new BlobResponseDto
        {
            Message = "Image uploaded successfully",
            IsSuccess = true,
            Blob = new BlobDto
            {
                FileUrl = blobClient.Uri.AbsoluteUri,
                FileName = blobClient.Name
            }
        };
    }

    public async Task<BlobResponseDto> DeleteAsync(string containerName,string filename)
    {
        
        var blobName = filename;
        var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainer.GetBlobClient(blobName);
        var response = await blobClient.DeleteIfExistsAsync();
            
        return new BlobResponseDto
        {
            Message = response ? "Image deleted successfully" : "Image not found",
            IsSuccess = response
         
        };
    }
    
    public async Task<BlobResponseDto> DownloadAsync(string containerName, string filename)
    {
        var blobName = filename;
        var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainer.GetBlobClient(blobName);
        var response = await blobClient.DownloadAsync();
        var blobDownloadInfo = response.Value;
        var content = string.Empty;
        using (var streamReader = new StreamReader(blobDownloadInfo.Content))
        {
            content = await streamReader.ReadToEndAsync();
        }
        return new BlobResponseDto
        {
            Message = content,
            IsSuccess = true
        };
    }

}