namespace PhoneVerification.Models
{
  public class SendMessageOptions
  {
    public string Body { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;

    private string _to = null!;

    public string To
    {
      get => $"+{_to}";
      set
      {
        _to = value.Contains("+") ? value.Replace("+", "") : value;
      }
    }
  }
}
