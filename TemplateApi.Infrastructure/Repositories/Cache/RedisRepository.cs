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

        public Task Add(string key, T value, TimeSpan? expiry = null)
        {
            string json = JsonConvert.SerializeObject(value);
            _database.StringSet(key, json, expiry);
            return Task.CompletedTask;
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
            RedisValue value = _database.StringGet(key);
            if (!value.HasValue)
            {
                return default;
            }

            string json = value.ToString();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public async Task SetAsync(string key, T value, TimeSpan? expiration)
        {
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.SetSlidingExpiration(expiration.Value);
            }
            else
            {
                options.SetSlidingExpiration(TimeSpan.FromSeconds(5));
            }

            var json = JsonConvert.SerializeObject(value);

            await _cache.SetStringAsync(key, json, options);
        }
    }
}

