namespace PhoneVerification.Models
{
  public class Verification
  {
    public string Identifier { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
  }
}
