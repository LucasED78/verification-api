using PhoneVerification.Models;

namespace PhoneVerification.Services.Interfaces
{
  public interface IMessageService
  {
    SendMessageResponse Send(SendMessageOptions options);

    Task<SendMessageResponse> SendAsync(SendMessageOptions options);
  }
}
