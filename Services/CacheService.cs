using Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RedisCache.WebAPI.Services.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RedisCache.WebAPI.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache redisCache;
        private readonly DistributedCacheEntryOptions cacheOptions;

        public CacheService(IDistributedCache redisCache, IConfiguration configuration)
        {
            this.redisCache = redisCache;
            this.cacheOptions = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromHours(configuration.GetValue<int>("CacheSlidingExpiration")),
                AbsoluteExpiration = DateTime.UtcNow.AddHours(configuration.GetValue<int>("CacheAbsoluteExpiration"))
            };
        }

        public async Task<List<T>> GetAsync<T>(string key)
        {
            var result = await redisCache.GetStringAsync(key) ?? throw new ArgumentNullException("key is null");
            var json = JsonConvert.DeserializeObject<List<T>>(result);
            return json;
        }

        public async Task<bool> SetAsync(string key, object value)
        {
            if (key == null || value == null)
            {
                throw new Exception("Invalid set cache request");
            }
            var obj = JsonConvert.SerializeObject(value);
            await redisCache.SetStringAsync(key, obj, cacheOptions);
            return true;
        }

        public async Task RemoveAsync(string key)
        {
            if (key == null)
            {
                throw new Exception("Invalid set cache request");
            }
            await redisCache.RemoveAsync(key);
        }

        public async Task ResetSlidingExpirationAsync(string key)
        {

            if (key == null)
            {
                throw new Exception("Invalid set cache request");
            }
            await redisCache.RefreshAsync(key);
        }
    }
}
