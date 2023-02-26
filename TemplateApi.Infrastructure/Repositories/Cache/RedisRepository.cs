using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using TemplateApi.Domain.Interfaces;

namespace TemplateApi.Infrastructure.Repositories.Cache
{
    public class RedisRepository<T> : IRedisRepository<T>
    {
        private readonly IDatabase _database;
        private readonly IDistributedCache _cache;

        public RedisRepository(IDistributedCache cache)
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _database = redis.GetDatabase();
            _cache = cache;
        }

        public void Add(string key, T value, TimeSpan? expiry = null)
        {
            string json = JsonConvert.SerializeObject(value);
            _database.StringSet(key, json, expiry);
        }

        public T Get(string key)
        {
            RedisValue result = _database.StringGet(key);
            if (!result.HasValue)
            {
                return default;
            }

            string json = result.ToString();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<T> GetAsync(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (value == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public async Task SetAsync(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }

            var json = JsonConvert.SerializeObject(value);

            await _cache.SetStringAsync(key, json, options);
        }
    }
}
