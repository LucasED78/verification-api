using PhoneVerification.Models;
using PhoneVerification.Repositories.Interfaces;
using PhoneVerification.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.Json;

namespace PhoneVerification.Services
{
  public class EmailMessageService : IMessageService<Verification, SendMessageOptions>
  {
    private readonly ISendGridClient _sendGridClient;
    private readonly IVerificationRepository<string> _verificationRepository;

    public EmailMessageService(ISendGridClient sendGridClient, IVerificationRepository<string> verificationRepository)
    {
      _sendGridClient = sendGridClient;
      _verificationRepository = verificationRepository;
    }

    public Verification? Get(string identifier)
    {
      var result = _verificationRepository.GetByIdentifier(identifier.ToLower());

      if (result != null)
      {
        return JsonSerializer.Deserialize<Verification>(result);
      }

      return null;
    }

    public async Task<Verification?> GetAsync(string identifier)
    {
      var result = await _verificationRepository.GetByIdentifierAsync(identifier.ToLower());

      if (result != null)
      {
        return JsonSerializer.Deserialize<Verification>(result);
      }

      return null;
    }

    public SendMessageResponse Send(SendMessageOptions options)
    {
      Verification verification;

      var existant = _verificationRepository.Exists(options.To.ToLower());

      if (existant)
      {
        verification = Get(options.To)!;
      } else
      {
        verification = new Verification { Identifier = options.To.ToLower() };

        _verificationRepository.Save(options.To.ToLower(), JsonSerializer.Serialize(verification));
      }

      var msg = MailHelper.CreateSingleEmail(
        from: new EmailAddress(options.From.ToLower()),
        subject: "This is your verification code",
        to: new EmailAddress(options.To.ToLower()),
        plainTextContent: "",
        htmlContent: options.Body.Replace("{{code}}", verification.Code)
      );

      _sendGridClient.SendEmailAsync(msg);

      return new SendMessageResponse { Code = verification.Code };
    }

    public async Task<SendMessageResponse> SendAsync(SendMessageOptions options)
    {
      Verification verification;

      var hasExistantVerification = await _verificationRepository.ExistsAsync(options.To.ToLower());

      if (hasExistantVerification)
      {
        verification = (await GetAsync(options.To))!;
      }
      else
      {
        verification = new Verification { Identifier = options.To.ToLower() };

        await _verificationRepository.SaveAsync(options.To.ToLower(), JsonSerializer.Serialize(verification));
      }

      var msg = MailHelper.CreateSingleEmail(
        from: new EmailAddress(options.From),
        to: new EmailAddress(options.To.ToLower()),
        subject: "This is your verification code",
        plainTextContent: "",
        htmlContent: options.Body.Replace("{{code}}", verification.Code)
      );

      await _sendGridClient.SendEmailAsync(msg);

      return new SendMessageResponse { Code = verification.Code };
    }
  }
}
