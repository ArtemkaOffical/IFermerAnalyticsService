using IFermerAnalyticsService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IFermerAnalyticsService.Data
{
    public class AnalyticsDbContext : DbContext
    {
        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> opt) : base(opt)
        {
          //  Database.EnsureCreated();
        }

       public DbSet<Product> Products { get; set; }
       public DbSet<Ticket> Tickets { get; set; }


    }
}
