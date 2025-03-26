using AutoMapper.Configuration.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using StudyMate.Data;
using StudyMate.DTOs;
using StudyMate.DTOs.Authentication;
using StudyMate.Models;
using StudyMate.Repositories.Interfaces;

namespace StudyMate.Repositories.Implementaions.Authentication
{
    public class UserRepository(UserManager<AppUser> userManager
        , IRoleRepository roleRepository, ApplicationDbContext context) : IUserRepository
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
            string? role = await roleRepository.GetUserRole(user.Email!);
            var claims = new List<Claim>
            {
                new Claim("Full Name", user!.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role!)
            };
            return claims;

        }

        public async Task<bool> ResetPassword(AppUser appUser,ResetPassword resetPassword)
        {
            var passwordHashed=  userManager.PasswordHasher.HashPassword(appUser,resetPassword.NewPassword);
            appUser.PasswordHash = passwordHashed;
            var result = await userManager.UpdateAsync(appUser);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserAllUsers()
        {
            var result = await context.Users.ExecuteDeleteAsync();
            return result > 0 ;
        }


        public async Task<LoginResponse> LoginUser(AppUser appUser)
        {
            var user = await GetUserByEmail(appUser.Email!);
            if (user is null || !await userManager.CheckPasswordAsync(user, appUser.PasswordHash!))
                return new LoginResponse(false, Message:"Invalid email or password");

            if (user.EmailConfirmed == false)
                return new LoginResponse(false, Message: "Email not confirmed");

            return new LoginResponse(true,  Message :"Login successful");           
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
            var user = await GetUserByEmail(appUser.Email!);
            if (user is null)
                return null;
            context.Users.Update(appUser);
            await context.SaveChangesAsync();
            return appUser;
        }
    }
}
