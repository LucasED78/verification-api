using PhoneVerification.Models;

namespace PhoneVerification.Services.Interfaces
{
  public interface IMessageService<T>
  {
    SendMessageResponse Send(SendMessageOptions options);

    Task<SendMessageResponse> SendAsync(SendMessageOptions options);

    T? Get(string identifier);

    Task<T?> GetAsync(string identifier);
  }
}
