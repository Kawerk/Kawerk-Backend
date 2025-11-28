using Kawerk.Infastructure.DTOs;

namespace Kawerk.Application.Interfaces
{
    public interface IVehicleService
    {
        public Task<int> CreateVehicle(VehicleDTO vehicle);
        public Task<int> UpdateVehicle(Guid vehicleID,VehicleDTO vehicle);
        public Task<int> DeleteVehicle(Guid vehicleID);
        public Task<VehicleDTO?> GetVehicle(Guid vehicleID);
        public Task<List<VehicleDTO>?> GetVehicles();
    }
}
