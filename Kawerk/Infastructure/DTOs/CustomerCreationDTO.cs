namespace Kawerk.Infastructure.DTOs
{
    public class CustomerCreationDTO
    {
        public Guid CustomerID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
