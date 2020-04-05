using Microsoft.EntityFrameworkCore;
using WoodpeckerBot.Data.Entities;

namespace WoodpeckerBot
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<TGUser> TGUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 
            Database.EnsureCreated();
        }
    }
}