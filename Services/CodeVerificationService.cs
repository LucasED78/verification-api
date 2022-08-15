using PhoneVerification.Exceptions;
using PhoneVerification.Models;
using PhoneVerification.Repositories.Interfaces;
using PhoneVerification.Services.Interfaces;
using System.Diagnostics;
using System.Text.Json;

namespace PhoneVerification.Services
{
  public class CodeVerificationService : ICodeVerificationService
  {
    private readonly IVerificationRepository<string> _verificationRepository;

    public CodeVerificationService (IVerificationRepository<string> verificationRepository)
    {
      _verificationRepository = verificationRepository;
    }

    public bool Verify(string identifier, string code)
    {
      var result = _verificationRepository.GetByIdentifier(identifier);

      if (result != null)
      {
        var parsed = JsonSerializer.Deserialize<Verification>(result);

        if (parsed == null) throw new ResourceNotFoundException();

        if (parsed.IsVerified) throw new AlreadyVerifiedException("This resource is already verified");
        if (parsed.Code != code) throw new InvalidCodeException();

        parsed.IsVerified = true;

        _verificationRepository.Save(identifier, JsonSerializer.Serialize(parsed));

        return true;
      }

      return false;
    }

    public async Task<bool> VerifyAsync(string identifier, string code)
    {
      var result = await _verificationRepository.GetByIdentifierAsync(identifier);

      Debug.WriteLine($"adsdas {result} {identifier}");

      if (result != null)
      {
        var parsed = JsonSerializer.Deserialize<Verification>(result);

        if (parsed == null) throw new ResourceNotFoundException();

        if (parsed.IsVerified) throw new AlreadyVerifiedException("This resource is already verified");
        if (parsed.Code != code) throw new InvalidCodeException();

        parsed.IsVerified = true;

        await _verificationRepository.SaveAsync(identifier, JsonSerializer.Serialize(parsed));

        return true;
      }

      return false;
    }
  }
}
