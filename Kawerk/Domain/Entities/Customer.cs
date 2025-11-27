using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Domain.Entities
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
        [Column(TypeName ="varchar(200)")]
        public string? ProfileUrl { get; set; }

        //Relationships
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<Transaction> Purchases { get; set; } = new List<Transaction>();
        public List<Transaction> Sells { get; set; } = new List<Transaction>();

    }
}
