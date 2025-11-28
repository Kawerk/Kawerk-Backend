namespace Kawerk.Infastructure.DTOs
{
    public class CustomerCreationDTO
    {
        public Guid CustomerID { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
