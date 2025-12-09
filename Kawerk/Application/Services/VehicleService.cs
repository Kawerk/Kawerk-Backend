using Kawerk.Application.Interfaces;
using Kawerk.Domain;
using Kawerk.Infastructure.Context;
using Kawerk.Infastructure.DTOs.Vehicle;
using Microsoft.EntityFrameworkCore;

namespace Kawerk.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly DbBase _db;
        public VehicleService(DbBase db)
        {
            _db = db;
        }

        //
        //        *********** Setters ***********
        public async Task<int> CreateVehicle(VehicleDTO vehicle)//0 == Faulty DTO || 1 == Successful
        {
            if (vehicle == null)
                return 0;

            Vehicle newVehicle = new Vehicle
            {
                VehicleID = Guid.NewGuid(),
                Name = vehicle.Name,
                Description = vehicle.Description,
                Price = vehicle.Price,
                Type = vehicle.Type,
                Transmission = vehicle.Transmission,    
                FuelType = vehicle.FuelType,
                EngineCapacity = vehicle.EngineCapacity,
                CreatedAt = DateTime.Now,
                SeatingCapacity = vehicle.SeatingCapacity,
                Status = vehicle.Status,
                Year = vehicle.Year,
            };

            await _db.Vehicles.AddAsync(newVehicle);
            await _db.SaveChangesAsync();
            return 1;

        }
        public async Task<int> UpdateVehicle(Guid vehicleID,VehicleDTO vehicle)//0 == Faulty DTO || 1 == Vehicle not found || 2 == Successful
        {
            //Checking DTO validity
            if (vehicle == null)
                return 0;

            //Getting Vehicle from Database
            var isVehicleExists = await (from v in _db.Vehicles
                                         where v.VehicleID == vehicleID
                                         select v).FirstOrDefaultAsync();
            //If vehicle not found return
            if (isVehicleExists == null)
                return 1;

            // --***Updating***--
            
            if(!string.IsNullOrEmpty(vehicle.Name))
                isVehicleExists.Name = vehicle.Name; 

            if(!string.IsNullOrEmpty(vehicle.Description))
                isVehicleExists.Description = vehicle.Description;

            if(!string.IsNullOrEmpty(vehicle.Transmission))
                isVehicleExists.Transmission = vehicle.Transmission;    

            if(!string.IsNullOrEmpty(vehicle.EngineCapacity))
                isVehicleExists.EngineCapacity = vehicle.EngineCapacity;

            if(!string.IsNullOrEmpty(vehicle.FuelType))
                isVehicleExists.FuelType = vehicle.FuelType;

            if(vehicle.Price != 0)
                isVehicleExists.Price = vehicle.Price;

            if(vehicle.SeatingCapacity != 0)
                isVehicleExists.SeatingCapacity = vehicle.SeatingCapacity;

            if(!string.IsNullOrEmpty(vehicle.Status))
                isVehicleExists.Status = vehicle.Status;

            if(!string.IsNullOrEmpty(vehicle.Type))
                isVehicleExists.Type = vehicle.Type;

            //Saving to Database
            _db.Vehicles.Update(isVehicleExists);
            await _db.SaveChangesAsync();
            return 2;
        }
        public async Task<int> DeleteVehicle(Guid vehicleID)//0 == Faulty ID || 1 == Vehicle not found || 2 == Successful
        {
            //Checking ID validity
            if (vehicleID == Guid.Empty)
                return 0;

            //Getting vehicle from Database
            var isVehicleExists = await (from v in _db.Vehicles
                                         where v.VehicleID == vehicleID
                                         select v).FirstOrDefaultAsync();
            //If vehicle not found return
            if (isVehicleExists == null)
                return 1;

            //Saving to Database
            _db.Vehicles.Remove(isVehicleExists);
            await _db.SaveChangesAsync();
            return 2;
        }

        //--------------------------------------------

        //        *********** Getters ***********
        public async Task<VehicleDTO?> GetVehicle(Guid vehicleID)
        {
            //Checking ID validity
            if (vehicleID == Guid.Empty)
                return null;

            //Getting the vehicle from the Database
            var isVehicleExists = await(from v in _db.Vehicles
                                        where v.VehicleID == vehicleID
                                        select new VehicleDTO
                                        {
                                            VehicleID = v.VehicleID,
                                            Name = v.Name,
                                            Description = v.Description,
                                            Price = v.Price,
                                            Type = v.Type,
                                            Transmission = v.Transmission,
                                            FuelType = v.FuelType,
                                            EngineCapacity = v.EngineCapacity,
                                            SeatingCapacity = v.SeatingCapacity,
                                            Status = v.Status,
                                            Year = v.Year,
                                        }).FirstOrDefaultAsync();
            //Returning the result
            return isVehicleExists;
        }
        public async Task<List<VehicleDTO>?> GetVehicles()
        {
            //Getting vehicles from the Database
            var vehicleQuery = await(from v in _db.Vehicles
                                     select new VehicleDTO
                                     {
                                         VehicleID = v.VehicleID,
                                         Name = v.Name,
                                         Description = v.Description,
                                         Price = v.Price,
                                         Type = v.Type,
                                         Transmission = v.Transmission,
                                         FuelType = v.FuelType,
                                         EngineCapacity = v.EngineCapacity,
                                         SeatingCapacity = v.SeatingCapacity,
                                         Status = v.Status,
                                         Year = v.Year,
                                     }).ToListAsync();
            //Returning the result
            return vehicleQuery;
        }
    }
}
