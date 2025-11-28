namespace Kawerk.Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<int> CreateCustomer();
        public Task<int> UpdateCustomer();
        public Task<int> DeleteCustomer();
    }
}
