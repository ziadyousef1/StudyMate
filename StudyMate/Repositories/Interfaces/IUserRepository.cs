using System.Security.Claims;
using StudyMate.DTOs;
using StudyMate.DTOs.Authentication;
using StudyMate.Models;

namespace StudyMate.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserResult> CreateUser(AppUser appAppUser);
    Task<LoginResponse> LoginUser(AppUser appAppUser);
    Task<AppUser?> GetUserByEmail(string email);
    Task<AppUser?> GetUserById(string id);
    Task<IEnumerable<AppUser>> GetAllUsers();
    Task<AppUser?> UpdateUser(AppUser appAppUser);
    Task<int> RemoveUserByEmail(string email);
    Task<List<Claim>> GetUserClaims(string email);
    Task<bool> ResetPassword(AppUser appUser,ResetPassword resetPassword);
    Task<bool> DeleteUserAllUsers();
}