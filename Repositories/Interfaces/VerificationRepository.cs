namespace PhoneVerification.Repositories.Interfaces
{
  public interface IVerificationRepository<T>
  {
    public bool Save(string key, T data);

    public T? GetByIdentifier(string identifier);

    public Task<bool> SaveAsync(string key, T data);

    public Task<T?> GetByIdentifierAsync(string identifier);
  }
}
