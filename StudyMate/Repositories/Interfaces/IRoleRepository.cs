namespace StudyMate.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<string?> GetUserRole(string userEmail);
    Task<bool> AddUserToRole(AppUser appUser, string roleName);
    Task<bool> RemoveUserFromRole(AppUser appUser, string roleName);

}