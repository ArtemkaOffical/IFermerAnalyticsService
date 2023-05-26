using IFermerAnalyticsService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IFermerAnalyticsService.Data
{
    public class AnalyticsDbContext : DbContext
    {
        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        DbSet<Product> Products { get; set; }
        DbSet<Ticket> Tickets { get; set; }


    }
}
