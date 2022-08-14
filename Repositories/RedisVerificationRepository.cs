using PhoneVerification.Repositories.Interfaces;
using StackExchange.Redis;

namespace PhoneVerification.Repositories
{
  public class RedisVerificationRepository : IVerificationRepository<string>
  {
    private readonly IDatabase _database;

    public RedisVerificationRepository(IConnectionMultiplexer redisConnection)
    {
      _database = redisConnection.GetDatabase();
    }

    public string? GetByIdentifier(string identifier)
    {
      return _database.StringGet(identifier);
    }

    public async Task<string?> GetByIdentifierAsync(string identifier)
    {
      var response = await _database.StringGetAsync(identifier);

      return response;
    }

    public bool Save(string key, string data)
    {
      return _database.StringSet(key, data);
    }

    public Task<bool> SaveAsync(string key, string data)
    {
      return _database.StringSetAsync(key, data);
    }

    public bool Exists(string key) => _database.KeyExists(key);

    public Task<bool> ExistsAsync(string key) => _database.KeyExistsAsync(key);
  }
}
