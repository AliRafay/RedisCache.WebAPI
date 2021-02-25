using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisCache.WebAPI.DataTransferObjects;
using RedisCache.WebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCache.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService cacheService;

        public CacheController(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        //[HttpPost("get")]
        //public async Task<IActionResult> GetAsync([FromBody] string key) => Ok(await cacheService.GetAsync(key));

        [HttpPost("set")]
        public async Task<IActionResult> SetAsync([FromBody] CacheSetDTO dto) => Ok(await cacheService.SetAsync(dto.Key, dto.Value));

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveAsync([FromBody] string key)
        {
            await cacheService.RemoveAsync(key);
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] string key)
        {
            await cacheService.ResetSlidingExpirationAsync(key);
            return Ok();
        }
    }
}
