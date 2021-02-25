using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCache.WebAPI.Services.Interfaces
{ 
    public interface ICacheService
    {
        Task<List<T>> GetAsync<T>(string key);
        Task RemoveAsync(string key);
        Task ResetSlidingExpirationAsync(string key);
        Task<bool> SetAsync(string key, object value);
    }
}
