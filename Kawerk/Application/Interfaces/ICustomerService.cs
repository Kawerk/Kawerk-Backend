using Kawerk.Infastructure.DTOs.Customer;
using Kawerk.Infastructure.ResponseClasses;
using Kawerk.Infastructure.DTOs.Vehicle;

namespace Kawerk.Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<SettersResponse> CreateCustomer(CustomerCreationDTO customer);
        public Task<SettersResponse> UpdateCustomer(Guid customerID, CustomerUpdateDTO customer);
        public Task<SettersResponse> DeleteCustomer(Guid customerID);
        public Task<SettersResponse> BuyVehicle(Guid customerID, Guid vehicleID);
        public Task<SettersResponse> SellVehicle(Guid sellerID, Guid vehicleID);
        public Task<CustomerViewDTO?> GetCustomer(Guid customerID);
        public Task<List<Kawerk.Infastructure.DTOs.Vehicle.VehicleViewDTO>?> GetBoughtVehicles(Guid customerID);
        public Task<List<Kawerk.Infastructure.DTOs.Vehicle.VehicleViewDTO>?> GetSoldVehicles(Guid customerID);
        public Task<List<CustomerViewDTO>?> GetCustomers();
    }
}
