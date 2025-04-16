using AutoMapper;
using StudyMate.DTOs.Profile;

namespace StudyMate.Services.Implementaions;

public class UserService(IUserRepository userRepository ,IMapper mapper): IUserService
{
    public async Task<GetUser?> CreateUserAsync(CreateUser createUser)
    {
        var mappedUser = mapper.Map<AppUser>(createUser);
        mappedUser.PasswordHash=createUser.Password;
        mappedUser.UserName = createUser.Email;
        var user = await userRepository.CreateUser(mappedUser);
        return mapper.Map<GetUser>(mappedUser);   
    }

    public async Task<GetUser?> GetUserAsync(string userId)
    {
        var user = await userRepository.GetUserById(userId);
        return mapper.Map<GetUser>(user);
        
    }

    public async Task<GetUser?> UpdateUserAsync(UpdateUser updateUser)
    {
        var user = await userRepository.GetUserById(updateUser.UserID);
        if (user is null)
          return null;
        var mappedUser = mapper.Map<AppUser>(updateUser);
        var updatedUser = await userRepository.UpdateUser(mappedUser);
        return mapper.Map<GetUser>(updatedUser);
       
    }

    public async Task<bool> DeleteUserAsync(DeleteUser deleteUser)
    {
        var user =await userRepository.GetUserById(deleteUser.UserId);
        if (user is null)
          return false;
        var result = await userRepository.RemoveUserByEmail(user.Email);
        return result > 0;

    }

    public async Task<int> UpdatePointsAsync(string userId, int points)
    {
        var user =await userRepository.GetUserById(userId);
        if (user is null)
          return 0;
        user.Points += points;  
        var result = await userRepository.UpdateUser(user);
        return result.Points;
    }
}