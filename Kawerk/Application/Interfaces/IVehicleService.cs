using Kawerk.Infastructure.DTOs.Vehicle;
using Kawerk.Infastructure.ResponseClasses;

namespace Kawerk.Application.Interfaces
{
    public interface IVehicleService
    {
        public Task<SettersResponse> CreateVehicle(VehicleDTO vehicle);
        public Task<SettersResponse> UpdateVehicle(Guid vehicleID,VehicleDTO vehicle);
        public Task<SettersResponse> DeleteVehicle(Guid vehicleID);
        public Task<VehicleDTO?> GetVehicle(Guid vehicleID);
        public Task<List<VehicleDTO>?> GetVehicles();
    }
}
