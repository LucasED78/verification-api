namespace PhoneVerification.Models
{
  public class SendMessageOptions
  {
    public string Body { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;

    public string To { get; set; } = string.Empty;
  }
}
