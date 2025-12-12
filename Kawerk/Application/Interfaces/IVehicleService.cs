using Kawerk.Infastructure.DTOs.Vehicle;
using Kawerk.Infastructure.ResponseClasses;

namespace Kawerk.Application.Interfaces
{
    public interface IVehicleService
    {
        public Task<SettersResponse> CreateVehicle(Infastructure.DTOs.Vehicle.VehicleViewDTO vehicle);
        public Task<SettersResponse> UpdateVehicle(Guid vehicleID, Infastructure.DTOs.Vehicle.VehicleViewDTO vehicle);
        public Task<SettersResponse> DeleteVehicle(Guid vehicleID);
        public Task<Infastructure.DTOs.Vehicle.VehicleViewDTO?> GetVehicle(Guid vehicleID);
        public Task<List<Infastructure.DTOs.Vehicle.VehicleViewDTO>?> GetVehicles();
    }
}
