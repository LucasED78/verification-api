using PhoneVerification.Models;

namespace PhoneVerification.Services.Interfaces
{
  public interface IMessageService<T, R> where R : SendMessageOptions
  {
    SendMessageResponse Send(R options);

    Task<SendMessageResponse> SendAsync(R options);

    T? Get(string identifier);

    Task<T?> GetAsync(string identifier);
  }
}
