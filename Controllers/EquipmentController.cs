using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisCache.WebAPI.DataTransferObjects;
using RedisCache.WebAPI.Entities;
using RedisCache.WebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCache.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private ICacheService cacheService;
        private readonly SoSDbContext context;

        public EquipmentController(ICacheService cacheService, SoSDbContext context)
        {
            this.cacheService = cacheService;
            this.context = context;
        }

        [HttpGet]
        public async Task<List<Equipment>> GetAsync()
        {
            return await cacheService.GetAsync<Equipment>("equipment");
        }

        [HttpGet("setcache")]
        public async Task<IActionResult> SetAsync()
        {
            var list = await context.Equipments.Select(x => new
            {
                x.Name,
                x.SectionName,
                x.WorkshopLevel
            }).ToListAsync();

            return Ok(await cacheService.SetAsync("equipment", list ));
        }

        [HttpGet("removecache")]
        public async Task<IActionResult> RemoveAsync()
        {
            await cacheService.RemoveAsync("equipment");
            return Ok();
        }

        [HttpGet("refreshcache")]
        public async Task<IActionResult> RefreshAsync()
        {
            await cacheService.ResetSlidingExpirationAsync("equipment");
            return Ok();
        }
    }
}
