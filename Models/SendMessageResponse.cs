namespace PhoneVerification.Models
{
  public class SendMessageResponse
  {
    public string Status { get; set; } = null!;
    public string? ErrorMessage { get; set; }
  }
}
