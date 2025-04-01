using StudyMate.DTOs.Profile;

namespace StudyMate.Services.Interfaces;

public interface IUserService
{
    Task<GetUser?> CreateUserAsync(CreateUser createUser);
    Task<GetUser?> GetUserAsync(string userId);
    Task<GetUser?> UpdateUserAsync(UpdateUser updateUser);
    Task<bool> DeleteUserAsync(DeleteUser deleteUser);
    Task<int> UpdatePointsAsync(string userId, int points);
    
    
}