using Kawerk.Infastructure.DTOs.Salesman;

namespace Kawerk.Application.Interfaces
{
    public interface ISalesmanService
    {
        public Task<int> CreateSalesman(SalesmanCreationDTO salesman);
        public Task<int> UpdateSalesman(Guid salesmanID,SalesmanUpdateDTO salesman);
        public Task<int> DeleteSalesman(Guid salesmanID);
        public Task<SalesmanViewDTO> GetSalesman(Guid salesmanID);
        public Task<List<SalesmanViewDTO>> GetSalesmen();
    }
}
