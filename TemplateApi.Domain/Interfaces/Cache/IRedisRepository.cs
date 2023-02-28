using System;
using System.Threading.Tasks;

namespace TemplateApi.Domain.Interfaces
{
    public interface IRedisRepository<T>
    {
        Task<T> GetAsync(string key);
        Task SetAsync(string key, T value, TimeSpan? expiration = null);
        T Get(string key);
        Task Add(string key, T value, TimeSpan? expiry = null);
        void Remove(string key);
    }
}
