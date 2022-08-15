using PhoneVerification.Exceptions;
using PhoneVerification.Models;
using PhoneVerification.Repositories.Interfaces;
using PhoneVerification.Services.Interfaces;
using System.Diagnostics;
using System.Text.Json;
using Twilio.Rest.Api.V2010.Account;

namespace PhoneVerification.Services
{
  public class SMSService : IMessageService<Verification, SendMessageOptions>
  {
    private readonly IVerificationRepository<string> _verificationRepository;

    public SMSService(IVerificationRepository<string> verificationRepository)
    {
      _verificationRepository = verificationRepository;
    }

    public Verification? Get(string identifier)
    {
      var result = _verificationRepository.GetByIdentifier(identifier.Contains("+") ? identifier : $"+{identifier}");

      if (result != null)
      {
        var deserialized = JsonSerializer.Deserialize<Verification>(result);

        return deserialized;
      }

      return null;
    }

    public async Task<Verification?> GetAsync(string identifier)
    {
      var result = await _verificationRepository.GetByIdentifierAsync(identifier.Contains("+") ? identifier : $"+{identifier}");

      if (result != null)
      {
        var deserialized = JsonSerializer.Deserialize<Verification>(result);

        return deserialized;
      }

      return null;
    }

    public SendMessageResponse Send(SendMessageOptions options)
    {
      Verification verification;
      var to = options.To.Contains("+") ? options.To : $"+{options.To}";

      var hasExistantVerification = _verificationRepository.Exists(to);

      if (hasExistantVerification)
      {
        verification = Get(to)!;
        if (verification.IsVerified) throw new AlreadyVerifiedException("This resource is already verified");
      }
      else
      {
        verification = new Verification
        {
          Identifier = to
        };

        _verificationRepository.Save(to, JsonSerializer.Serialize(verification));
      }

      var result = MessageResource.Create(
        body: options.Body.Replace("{{code}}", verification.Code),
        from: new Twilio.Types.PhoneNumber(options.From),
        to: new Twilio.Types.PhoneNumber(to)
      );

      return new SendMessageResponse
      {
        Code = verification.Code,
        ErrorMessage = result.ErrorMessage
      };
    }

    public async Task<SendMessageResponse> SendAsync(SendMessageOptions options)
    {
      Verification verification;
      var to = options.To.Contains("+") ? options.To : $"+{options.To}";

      var hasExistantVerification = await _verificationRepository.ExistsAsync(to);

      if (hasExistantVerification)
      {
        verification = (await GetAsync(to))!;

        if (verification.IsVerified) throw new AlreadyVerifiedException("This resource is already verified");
      } else
      {
        verification = new Verification
        {
          Identifier = to
        };

        await _verificationRepository.SaveAsync(to, JsonSerializer.Serialize(verification));
      }

      var result = await MessageResource.CreateAsync(
        body: options.Body.Replace("{{code}}", verification.Code),
        from: new Twilio.Types.PhoneNumber(options.From),
        to: new Twilio.Types.PhoneNumber(to)
      );

      return new SendMessageResponse
      {
        Code = verification.Code,
        ErrorMessage = result.ErrorMessage
      };
    }
  }
}
