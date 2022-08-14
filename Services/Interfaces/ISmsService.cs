using PhoneVerification.Models;

namespace PhoneVerification.Services.Interfaces
{
  public interface ISmsService : IMessageService<SmsVerification>
  {
    bool Verify(string identifier, string code);

    Task<bool> VerifyAsync(string identifier, string code);
  }
}
