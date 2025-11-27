using Kawerk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kawerk
{
    public class DbBase : DbContext
    {
        public DbBase(DbContextOptions options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Branches> Branches { get; set; }
        public DbSet<Salesman> Salesman { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<Vehicle>();
            modelBuilder.Entity<Manufacturer>();
            modelBuilder.Entity<Branches>();
            modelBuilder.Entity<Salesman>();

            //Transaction Buyer Foreign Key
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Buyer)
                .WithMany(c => c.Purchases)
                .HasForeignKey(t => t.BuyerID);

            //Transaction Vehicle Foreign Key
            modelBuilder.Entity<Transaction>()
                .HasOne(t=>t.Vehicle)
                .WithOne(v=>v.Transaction)
                .HasForeignKey<Transaction>(t=>t.VehicleID);

            //Transaction SellerCustomer Foreign Key
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SellerCustomer)
                .WithMany(c => c.Sells)
                .HasForeignKey(t => t.SellerCustomerID);

            //Transaction SellerManufacturer Foreign Key
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SellerManufacturer)
                .WithMany(m => m.Transactions)
                .HasForeignKey(t => t.SellerManufacturerID);
        }

    }
}
