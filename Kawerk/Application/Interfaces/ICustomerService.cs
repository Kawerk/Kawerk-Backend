using Kawerk.Infastructure.DTOs.Customer;
using Kawerk.Infastructure.ResponseClasses;

namespace Kawerk.Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<SettersResponse> CreateCustomer(CustomerCreationDTO customer);
        public Task<SettersResponse> UpdateCustomer(Guid customerID, CustomerUpdateDTO customer);
        public Task<SettersResponse> DeleteCustomer(Guid customerID);
        public Task<CustomerViewDTO?> GetCustomer(Guid customerID);
        public Task<List<CustomerViewDTO>?> GetCustomers();
    }
}
