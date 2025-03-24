using Microsoft.EntityFrameworkCore;
using StudyMate.Data;
using StudyMate.Models;
using StudyMate.Repositories.Interfaces;

namespace StudyMate.Repositories.Implementaions;

public class VerificationCodeRepository:IVerificationCodeRepository
{
    private readonly ApplicationDbContext _context;

    public VerificationCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task StoreVerificationCode(string userId, int code)
    {
        
        var verificationCode = new VerificationCode
        {
            UserId = userId,
            Code = code,
            ExpirationTime = DateTime.UtcNow.AddHours(5)
        };
        _context.VerificationCodes.Add(verificationCode);
        await _context.SaveChangesAsync();
    }


    public async Task<bool> VerifyCode(int code)
    {
        var verificationCode = await _context.VerificationCodes
            .FirstOrDefaultAsync(x => x.Code == code);

        if (verificationCode == null || verificationCode.ExpirationTime < DateTime.UtcNow)
            return false;
        return true;
    }
    public async Task DeleteVerificationCode(int code)
    {
        var verificationCode = await _context.VerificationCodes
            .FirstOrDefaultAsync(x => x.Code == code);

        if (verificationCode != null)
        {
            _context.VerificationCodes.Remove(verificationCode);
            await _context.SaveChangesAsync();
        }
    }
}