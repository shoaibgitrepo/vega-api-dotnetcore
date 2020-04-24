using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Models;

namespace vega_api_dotnetcore.Persistence
{
    public class VegaDbContext : DbContext
    {
        public VegaDbContext(DbContextOptions<VegaDbContext> options) : base(options)
        {
        }

        public DbSet<Make> Makes { get; set; }
        public DbSet<Feature> Features { get; set; }
    }
}