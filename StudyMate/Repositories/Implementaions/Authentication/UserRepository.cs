using AutoMapper.Configuration.Annotations;
using EcommerceApp.Domain.Entities.Identity;
using EcommerceApp.Domain.Interfaces.Authentication;
using EcommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceApp.Infrastructure.Repositories.Authentication
{
    public class UserRepository(UserManager<AppUser> userManager
        , IRoleManagement roleManagement, AppDbContext context) : IUserManagement
    {
        public async Task<bool> CreateUser(AppUser appUser)
        {
            var user =await GetUserByEmail(appUser.Email!);
            if (user is not null) return false;
            var result = await userManager.CreateAsync(appUser, appUser.PasswordHash!);
           
            return result.Succeeded;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
            => await context.Users.ToListAsync();


        public async Task<AppUser?> GetUserByEmail(string email)
            => await userManager.FindByEmailAsync(email);


        public async Task<AppUser?> GetUserById(string id)
         => await userManager.FindByIdAsync(id);

        public async Task<List<Claim>> GetUserClaims(string email)
        {
            var user = await GetUserByEmail(email);
            string? role = await roleManagement.GetUserRole(user.Email!);
            var claims = new List<Claim>
            {
                new Claim("Full Name", user!.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role!)
            };
            return claims;

        }

        public async Task<bool> LoginUser(AppUser appUser)
        {
            var user = await GetUserByEmail(appUser.Email!);
            if (user is null)
                return false;
            var roleName = await roleManagement.GetUserRole(user.Email!);
            if(string.IsNullOrEmpty(roleName))
                return false;

            return await userManager.CheckPasswordAsync(user, appUser.PasswordHash!);           

            
        }

        public async Task<int> RemoveUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(_ => _.Email==email);
            if (user is null)
                return 0;

            context.Users.Remove(user);
            return await context.SaveChangesAsync();
        }

        public async Task<AppUser?> UpdateUser(AppUser appUser)
        {
            var user = GetUserByEmail(appUser.Email!);
            if (user is null)
                return null;
             context.Users.Update(appUser);
            await context.SaveChangesAsync();
            return appUser;
        }
    }
}
