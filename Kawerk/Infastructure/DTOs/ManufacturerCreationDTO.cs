namespace Kawerk.Infastructure.DTOs
{
    public class ManufacturerCreationDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; } // Cars or motorcycles , etc
    }
}
