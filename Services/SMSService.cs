using PhoneVerification.Models;
using PhoneVerification.Repositories.Interfaces;
using PhoneVerification.Services.Interfaces;
using System.Text.Json;
using Twilio.Rest.Api.V2010.Account;

namespace PhoneVerification.Services
{
  public class SMSService : IMessageService
  {
    private IVerificationRepository<string> _verificationRepository;

    public SMSService(IVerificationRepository<string> verificationRepository)
    {
      _verificationRepository = verificationRepository;
    }

    public SendMessageResponse Send(SendMessageOptions options)
    {
      var result = MessageResource.Create(
        body: options.Body,
        from: new Twilio.Types.PhoneNumber(options.From),
        to: new Twilio.Types.PhoneNumber(options.To)
      );
      
      _verificationRepository.Save(options.To, JsonSerializer.Serialize(new Verification
      {
        Identifier = options.To
      }));

      return new SendMessageResponse
      {
        Status = "Pending",
        ErrorMessage = result.ErrorMessage
      };
    }

    public async Task<SendMessageResponse> SendAsync(SendMessageOptions options)
    {
      var result = await MessageResource.CreateAsync(
        body: options.Body,
        from: new Twilio.Types.PhoneNumber(options.From),
        to: new Twilio.Types.PhoneNumber(options.To)
      );

      await _verificationRepository.SaveAsync(options.To, JsonSerializer.Serialize(new Verification
      {
        Identifier = options.To,
      }));

      return new SendMessageResponse
      {
        Status = "Pending",
        ErrorMessage = result.ErrorMessage
      };
    }
  }
}
