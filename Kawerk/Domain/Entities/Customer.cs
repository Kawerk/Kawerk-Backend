using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Domain.Entities
{
    public class Customer
    {
        [Key]
        public Guid CustomerID { get; set; }
        [Column(TypeName ="nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Password { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Phone { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Address { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string City { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Region { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string PostalCode { get; set; }
        [Column(TypeName ="varchar(200)")]
        public string ProfileUrl { get; set; }

        //Relationships
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    }
}
