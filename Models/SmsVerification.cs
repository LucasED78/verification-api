using System.Security.Cryptography;

namespace PhoneVerification.Models
{
  public class SmsVerification : Verification
  {
    public string Code { get; set; } = RandomNumberGenerator.GetInt32(0, 1000000).ToString().PadLeft(6, '0');
  }
}
