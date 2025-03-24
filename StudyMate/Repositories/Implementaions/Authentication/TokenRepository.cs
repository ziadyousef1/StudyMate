using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EcommerceApp.Domain.Entities.Identity;
using EcommerceApp.Domain.Interfaces.Authentication;
using EcommerceApp.Infrastructure.Data;
using EcommerceApp.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceApp.Infrastructure.Repositories.Authentication
{
    public class TokenRepository(AppDbContext context ,IOptions<JwtSettings> jwtSettings) : ITokenManagement
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        public async Task<int> AddRefreshToken(string userId, string refreshToken)
        {
            context.RefreshTokens.Add(
                new RefreshToken()
                {
                    userId = userId,
                    Token = refreshToken,
                });
            return await context.SaveChangesAsync();
        }

        public string GenerateRefreshToken()
        {
            const int byteSize= 64;
            var randomNumber = new byte[byteSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);

        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expairation = DateTime.UtcNow.AddHours(2);
            var token = new JwtSecurityToken(
                issuer:_jwtSettings.Issuer,
                audience:_jwtSettings.Audience,
                claims:claims,
                expires: expairation,
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public List<Claim> GetClaimsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return token != null  ? jwtToken.Claims.ToList():[];   
        }

        public async Task<string> GetUserIdByRefreshToken(string RefreshToken)
         => await (from token in context.RefreshTokens
                   where token.Token == RefreshToken
                   select token.userId).FirstOrDefaultAsync();

        public async Task<int> UpdateRefreshToken(string userId, string refreshToken)
        {
            var user = await context.RefreshTokens.FirstOrDefaultAsync(_ => _.Token== refreshToken);

            if (user is null) return -1;
            user.Token = refreshToken;
            return await context.SaveChangesAsync();
        }

        public async Task<bool> ValidateRefreshToken(string RefreshToken)
        {
            var token = await context.RefreshTokens.FirstOrDefaultAsync(_ => _.Token == RefreshToken);
            return token != null;
        }
    }
}
