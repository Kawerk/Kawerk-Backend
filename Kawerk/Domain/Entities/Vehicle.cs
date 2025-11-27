using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public Guid VehicleID { get; set; }
        [Column(TypeName ="varchar(200)")]
        public string Name { get; set; }
        [Column(TypeName ="varchar(2000)")]
        public string Description { get; set; }
        public int price { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Type { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Transmission {  get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Year { get; set; }
        public int SeatingCapacity { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string EngineCapacity { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string FuelType { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Status { get; set; }
        public List<string> Images { get; set; }

        //Relationships
        [Required]
        public Manufacturer Manufacturer { get; set; }
        public Customer? Customer { get; set; }
        public Transaction Transaction { get; set; }
    }
}
