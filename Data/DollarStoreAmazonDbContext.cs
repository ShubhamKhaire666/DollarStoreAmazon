using DollarStoreAmazon.Models;
using Microsoft.EntityFrameworkCore;

namespace DollarStoreAmazon.Data
{
    public class DollarStoreAmazonDbContext : DbContext
    {
        public DollarStoreAmazonDbContext(DbContextOptions<DollarStoreAmazonDbContext> options) : base(options)
        {
        
        }

        public DbSet<Category> Categories{ get; set; }
    }
}
