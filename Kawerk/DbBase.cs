using Kawerk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kawerk
{
    public class DbBase : DbContext
    {
        public DbBase(DbContextOptions options) : base(options) { }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<Vehicle>();
            modelBuilder.Entity<Manufacturer>();

        }

    }
}
