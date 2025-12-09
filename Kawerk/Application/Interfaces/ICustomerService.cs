using Kawerk.Infastructure.DTOs.Customer;

namespace Kawerk.Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<int> CreateCustomer(CustomerCreationDTO customer);
        public Task<int> UpdateCustomer(Guid customerID, CustomerUpdateDTO customer);
        public Task<int> DeleteCustomer(Guid customerID);
        public Task<CustomerViewDTO?> GetCustomer(Guid customerID);
        public Task<List<CustomerViewDTO>?> GetCustomers();
    }
}
