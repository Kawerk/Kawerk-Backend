namespace Kawerk.Domain.Entities
{
    public class Transaction
    {
        public Guid TransactionID { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    
        //Relationships
        public Guid BuyerID { get; set; }
        public Guid VehicleID { get; set; }
        public Guid SellerCustomerID { get; set; }
        public Guid SellerManufacturerID { get; set;}
    }
}
