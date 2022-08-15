namespace PhoneVerification.Models
{
  public class SendPhoneMessageOptions : SendMessageOptions
  {
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
