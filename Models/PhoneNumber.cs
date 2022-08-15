namespace PhoneVerification.Models
{
  public class PhoneNumber
  {
    public string Value { get; private set; }

    public PhoneNumber(string value)
    {
      Value = value.Contains("+") ? value : $"+{value}";
    }
  }
}
