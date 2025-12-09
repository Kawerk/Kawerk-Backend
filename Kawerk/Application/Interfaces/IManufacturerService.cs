using Kawerk.Infastructure.DTOs.Manufacturer;
using Kawerk.Infastructure.ResponseClasses;

namespace Kawerk.Application.Interfaces
{
    public interface IManufacturerService
    {
        public Task<SettersResponse> CreateManufacturer(ManufacturerCreationDTO manufacturer);
        public Task<SettersResponse> UpdateManufacturer(Guid manufacturerID,ManufacturerUpdateDTO manufacturer);
        public Task<SettersResponse> DeleteManufacturer(Guid manufacturerID);
        public Task<ManufacturerViewDTO?> GetManufacturer(Guid manufacturerID);
        public Task<List<ManufacturerViewDTO>?> GetManufacturers();
    }
}
