using PhoneVerification.Repositories.Interfaces;
using StackExchange.Redis;

namespace PhoneVerification.Repositories
{
  public class RedisVerificationRepository : IVerificationRepository<string>
  {
    private readonly IDatabase _database;

    public RedisVerificationRepository(ConnectionMultiplexer redisConnection)
    {
      _database = redisConnection.GetDatabase();
    }

    public string? GetByIdentifier(string identifier)
    {
      return _database.StringGet(identifier).ToString();
    }

    public async Task<string?> GetByIdentifierAsync(string identifier)
    {
      var response = await _database.StringGetAsync(identifier);

      return response.ToString();
    }

    public bool Save(string key, string data)
    {
      return _database.SetAdd(key, data);
    }

    public Task<bool> SaveAsync(string key, string data)
    {
      return _database.SetAddAsync(key, data);
    }
  }
}
