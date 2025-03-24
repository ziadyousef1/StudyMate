
using Microsoft.AspNetCore.Identity;
using StudyMate.Models;
using StudyMate.Repositories.Interfaces;

namespace StudyMate.Repositories.Implementaions.Authentication
{
    public class RoleRepository(UserManager<AppUser> userManager) : IRoleRepository
    {
        public async Task<bool> AddUserToRole(AppUser user, string roleName)
        {
            var result = await userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }
        public async Task<string?> GetUserRole(string userEmail)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            return (await userManager.GetRolesAsync(user!)).FirstOrDefault();
        }

        public async Task<bool> RemoveUserFromRole(AppUser user, string roleName)
        => (await userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
    }
}
