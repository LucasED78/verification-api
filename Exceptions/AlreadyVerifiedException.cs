namespace PhoneVerification.Exceptions
{
  public class AlreadyVerifiedException : Exception
  {
    public AlreadyVerifiedException()
    {

    }

    public AlreadyVerifiedException(string message) : base(message)
    {

    }
  }
}
