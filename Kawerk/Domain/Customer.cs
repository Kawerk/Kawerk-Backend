using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Domain
{
    public class Customer
    {
        [Key]
        public Guid CustomerID { get; set; }
        [Column(TypeName ="nvarchar(100)")]
        public required string Name { get; set; }
        [Column(TypeName = "varchar(100)")]
        public required string Username { get; set; }
        [Column(TypeName = "varchar(100)")]
        public required string Email { get; set; }
        [Column(TypeName = "varchar(200)")]
        public required string Password { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Phone { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? Address { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? City { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Country { get; set; }
        public DateTime CreatedAt { get; set; }
        [Column(TypeName ="varchar(200)")]
        public string? ProfileUrl { get; set; }
        [Column(TypeName = "varchar(50)")]
        public required string Role { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? AdministratorOf { get; set; } //If the customer is a manufacturer admin, this field will store the Manufacturer Name they administer, This could also apply if the manage a branch

        //Relationships
        public List<Vehicle> VehiclesBought { get; set; } = new List<Vehicle>();
        public List<Vehicle> VehiclesSold { get; set; } = new List<Vehicle>();
        public List<Transaction> Purchases { get; set; } = new List<Transaction>();
        public List<Transaction> Sells { get; set; } = new List<Transaction>();
        public List<Manufacturer> SubscribedManufacturers { get; set; } = new List<Manufacturer>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();


    }
}
