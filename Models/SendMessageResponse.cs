namespace PhoneVerification.Models
{
  public class SendMessageResponse
  {
    public string Code { get; set; } = null!;
    public string? ErrorMessage { get; set; }
  }
}
