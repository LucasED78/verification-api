using System.Security.Cryptography;

namespace PhoneVerification.Models
{
  public class Verification
  {
    public string Identifier { get; set; } = string.Empty;
    public string Code { get; set; } = RandomNumberGenerator.GetInt32(0, 1000000).ToString().PadLeft(6, '0');
    public bool IsVerified { get; set; } = false;
  }
}
