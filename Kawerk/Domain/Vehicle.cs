using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Domain
{
    public class Vehicle
    {
        [Key]
        public Guid VehicleID { get; set; }
        [Column(TypeName ="varchar(200)")]
        public required string Name { get; set; }
        [Column(TypeName ="varchar(2000)")]
        public string? Description { get; set; }
        public int Price { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Type { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Transmission {  get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Year { get; set; }
        public int SeatingCapacity { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? EngineCapacity { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? FuelType { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string>? Images { get; set; }
        public string ManufacturerName => string.IsNullOrEmpty(Manufacturer.Name)? Seller.Name : Manufacturer.Name;

        //Relationships
        public Manufacturer? Manufacturer { get; set; }
        public Guid? ManufacturerID { get; set; }
        public Customer? Seller { get; set; }
        public Guid? SellerID { get; set; }
        public Customer? Buyer { get; set; }
        public Guid? BuyerID {  get; set; }
        public Transaction? Transaction { get; set; }
    }
}
