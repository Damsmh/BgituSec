using BgituSec.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BgituSec.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Auditorium> Auditoriums { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Breakdown> Breakdowns { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
