using Kawerk.Infastructure.DTOs.Manufacturer;

namespace Kawerk.Application.Interfaces
{
    public interface IManufacturerService
    {
        public Task<int> CreateManufacturer(ManufacturerCreationDTO manufacturer);
        public Task<int> UpdateManufacturer(Guid manufacturerID,ManufacturerUpdateDTO manufacturer);
        public Task<int> DeleteManufacturer(Guid manufacturerID);
        public Task<ManufacturerViewDTO?> GetManufacturer(Guid manufacturerID);
        public Task<List<ManufacturerViewDTO>?> GetManufacturers();
    }
}
