using Kawerk.Infastructure.DTOs.Customer;
using Kawerk.Infastructure.ResponseClasses;
using Kawerk.Infastructure.DTOs.Vehicle;
using Kawerk.Infastructure.DTOs.Notification;
using Kawerk.Infastructure.DTOs.Manufacturer;

namespace Kawerk.Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<SettersResponse> CreateCustomer(CustomerCreationDTO customer);
        public Task<SettersResponse> UpdateCustomer(Guid customerID, CustomerUpdateDTO customer);
        public Task<SettersResponse> DeleteCustomer(Guid customerID);
        public Task<SettersResponse> BuyVehicle(Guid customerID, Guid vehicleID);
        public Task<SettersResponse> SellVehicle(Guid sellerID, Guid vehicleID);
        public Task<SettersResponse> Subscribe(Guid customerID, Guid manufacturerID);
        public Task<CustomerViewDTO?> GetCustomer(Guid customerID);
        public Task<PagedList<CustomerViewDTO>?> GetFilteredCustomers(string startDate, string endDate, int page, string sortColumn, string OrderBy, string searchTerm, int pageSize);
        public Task<PagedList<VehicleViewDTO>?> GetBoughtVehicles(Guid customerID, string startDate, string endDate, int page, string sortColumn, string OrderBy, string searchTerm, int pageSize);
        public Task<PagedList<VehicleSellerViewDTO>?> GetSoldVehicles(Guid customerID, string startDate, string endDate, int page, string sortColumn, string OrderBy, string searchTerm, int pageSize);
        public Task<PagedList<ManufacturerViewDTO>?> GetSubscribedManufacturers(Guid customerID, int page, int pageSize);
        public Task<PagedList<NotificationViewDTO>?> GetNotifications(Guid customerID, int page, int pageSize);
        public Task<PagedList<CustomerViewDTO>?> GetCustomers(int page,int pageSize);
    }
}
