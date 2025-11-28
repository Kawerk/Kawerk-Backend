using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Infastructure.DTOs
{
    public class CustomerUpdateDTO
    {
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ProfileUrl { get; set; }
    }
}
