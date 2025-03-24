namespace StudyMate.Repositories.Interfaces;

public interface IVerificationCodeRepository
{
      Task StoreVerificationCode(string userId, int code);
      Task<bool> VerifyCode(int code);
      Task DeleteVerificationCode(int code);
      

}