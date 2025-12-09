using Kawerk.Infastructure.DTOs.Manufacturer;
using Kawerk.Infastructure.DTOs.Vehicle;
using Kawerk.Infastructure.ResponseClasses;

namespace Kawerk.Application.Interfaces
{
    public interface IManufacturerService
    {
        public Task<SettersResponse> CreateManufacturer(ManufacturerCreationDTO manufacturer);
        public Task<SettersResponse> UpdateManufacturer(Guid manufacturerID,ManufacturerUpdateDTO manufacturer);
        public Task<SettersResponse> DeleteManufacturer(Guid manufacturerID);
        public Task<SettersResponse> SellVehicle(Guid manufacturerID,Guid vehicleID);
        public Task<ManufacturerViewDTO?> GetManufacturer(Guid manufacturerID);
        public Task<VehicleManufacturerViewDTO?> GetSoldVehicles(Guid manufacturerID);
        public Task<List<ManufacturerViewDTO>?> GetManufacturers();
    }
}
