using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Infastructure.DTOs
{
    public class ManufacturerDTO
    {
        public Guid ManufacturerID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; } // Cars or motorcycles , etc
    }
}
