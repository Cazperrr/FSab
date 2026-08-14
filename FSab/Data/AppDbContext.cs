using FSab.Models;
using Microsoft.EntityFrameworkCore;

namespace FSab.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<FaultReport> FaultReports { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
