using Entities;
using Microsoft.EntityFrameworkCore;

namespace RedisCache.WebAPI.Entities
{
    public class SoSDbContext : DbContext
    {
        public SoSDbContext(DbContextOptions<SoSDbContext> options)
        : base(options)
        {
        }

        public DbSet<Equipment> Equipments { get; set; }
    }
}
