namespace PhoneVerification.Services.Interfaces
{
  public interface ICodeVerificationService
  {
    bool Verify(string identifier, string code);

    Task<bool> VerifyAsync(string identifier, string code);
  }
}
