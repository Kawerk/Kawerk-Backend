namespace Kawerk.Infastructure.DTOs
{
    public class ManufacturerUpdateDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; } // Cars or motorcycles , etc
        public string? Warranty { get; set; }
    }
}
