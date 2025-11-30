using Kawerk.Domain;

namespace Kawerk.Infastructure.DTOs
{
    public class TransactionDTO
    {
        public Guid TransactionID { get; set; }
        public int Amount { get; set; }
        public Guid BuyerID { get; set; }
        public Guid VehicleID { get; set; }
        public Guid? SellerCustomerID { get; set; }
        public Guid? SellerManufacturerID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
