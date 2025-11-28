using Kawerk.Infastructure.DTOs;

namespace Kawerk.Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<int> CreateCustomer(CustomerCreationDTO customer);
        public Task<int> UpdateCustomer(Guid customerID, CustomerUpdateDTO customer);
        public Task<int> DeleteCustomer(Guid customerID);
        public Task<CustomerDTO?> GetCustomer(Guid customerID);
        public Task<List<CustomerDTO>?> GetCustomers();
    }
}
