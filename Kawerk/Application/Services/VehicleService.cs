using Kawerk.Application.Interfaces;
using Kawerk.Domain;
using Kawerk.Infastructure.Context;
using Kawerk.Infastructure.DTOs.Vehicle;
using Kawerk.Infastructure.ResponseClasses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public async Task<SettersResponse> CreateVehicle(VehicleViewDTO vehicle)//0 == Faulty DTO || 1 == Successful
        {
            if (vehicle == null)
                return new SettersResponse { status = 0, msg = "Faulty DTO" };

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
            return new SettersResponse { status = 1, msg = "Vehicle created successfully" };
        }
        public async Task<SettersResponse> UpdateVehicle(Guid vehicleID, VehicleViewDTO vehicle)//0 == Faulty DTO || 1 == Vehicle not found || 2 == Successful
        {
            //Checking DTO validity
            if (vehicle == null)
                return new SettersResponse { status = 0, msg = "Faulty DTO" };

            //Getting Vehicle from Database
            var isVehicleExists = await (from v in _db.Vehicles
                                         where v.VehicleID == vehicleID
                                         select v).FirstOrDefaultAsync();
            //If vehicle not found return
            if (isVehicleExists == null)
                return new SettersResponse { status = 0, msg = "Vehicle not found" };

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
            return new SettersResponse { status = 1, msg = "Vehicle updated successfully" };
        }
        public async Task<SettersResponse> DeleteVehicle(Guid vehicleID)//0 == Faulty ID || 1 == Vehicle not found || 2 == Successful
        {
            //Checking ID validity
            if (vehicleID == Guid.Empty)
                return new SettersResponse { status = 0, msg = "Faulty ID" };

            //Getting vehicle from Database
            var isVehicleExists = await (from v in _db.Vehicles
                                         where v.VehicleID == vehicleID
                                         select v).FirstOrDefaultAsync();
            //If vehicle not found return
            if (isVehicleExists == null)
                return new SettersResponse { status = 0, msg = "Vehicle not found" };

            //Saving to Database
            _db.Vehicles.Remove(isVehicleExists);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Vehicle deleted successfully" };
        }

        //--------------------------------------------

        //        *********** Getters ***********
        public async Task<VehicleViewDTO?> GetVehicle(Guid vehicleID)
        {
            //Checking ID validity
            if (vehicleID == Guid.Empty)
                return null;

            //Getting the vehicle from the Database
            var isVehicleExists = await(from v in _db.Vehicles
                                        where v.VehicleID == vehicleID
                                        select new VehicleViewDTO
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
        public async Task<PagedList<VehicleViewDTO>?> GetFilteredVehicles(string startDate, string endDate, int minimumPrice, int maximumPrice, int page, string sortColumn, string OrderBy, string searchTerm, int pageSize)
        {
            IQueryable<Vehicle> vehiclesQuery = _db.Vehicles;
            DateTime validStartDate, validEndDate;
            if (DateTime.TryParse(startDate, out validStartDate))
            {
                vehiclesQuery = vehiclesQuery.Where(u => u.Transaction.CreatedDate > validStartDate);
            }
            if (DateTime.TryParse(endDate, out validEndDate))
            {
                vehiclesQuery = vehiclesQuery.Where(u => u.Transaction.CreatedDate < validEndDate);
            }

            if (minimumPrice > 0)
                vehiclesQuery = vehiclesQuery.Where(u => u.Price >= minimumPrice);

            if (maximumPrice > 0)
                vehiclesQuery = vehiclesQuery.Where(u => u.Price <= maximumPrice);

            if (!string.IsNullOrEmpty(searchTerm))
                vehiclesQuery = vehiclesQuery.Where(u => u.Name.Contains(searchTerm) ||
                u.Description.Contains(searchTerm) || u.Type.Contains(searchTerm) || u.FuelType.Contains(searchTerm));
            if (!string.IsNullOrEmpty(sortColumn))
            {
                Expression<Func<Vehicle, object>> keySelector = sortColumn.ToLower() switch // throws error when sortColumn is null
                {
                    "name" or "n" => Vehicle => Vehicle.Name,
                    "price" or "p" => Vehicle => Vehicle.Price,
                    "type" or "t" => Vehicle => Vehicle.Type,
                    "enginecapacity" or "ec" => Vehicle => Vehicle.EngineCapacity,
                    "fueltype" or "f" => Vehicle => Vehicle.FuelType,
                    "seatingcapacity" or "sc" => Vehicle => Vehicle.SeatingCapacity,
                    "status" or "s" => Vehicle => Vehicle.Status,
                    _ => Vehicle => Vehicle.VehicleID,
                };
                if (!string.IsNullOrEmpty(OrderBy)) vehiclesQuery = vehiclesQuery.OrderBy(keySelector);
                else vehiclesQuery = vehiclesQuery.OrderBy(keySelector);
            }
            var vehiclesResponse = vehiclesQuery
                                    .Select(v => new VehicleViewDTO
                                    {
                                        VehicleID = v.VehicleID,
                                        Name = v.Name,
                                        Description = v.Description,
                                        Price = v.Price,
                                        Type = v.Type,
                                        EngineCapacity = v.EngineCapacity,
                                        FuelType = v.FuelType,
                                        SeatingCapacity = v.SeatingCapacity,
                                        Transmission = v.Transmission,
                                        Year = v.Year,
                                        Status = v.Status,
                                        Images = v.Images
                                    });


            return await PagedList<VehicleViewDTO>.CreateAsync(vehiclesResponse, page, pageSize);
        }
        public async Task<PagedList<VehicleViewDTO>?> GetVehicles(int pageNumber, int pageSize)
        {
            //Getting vehicles from the Database
            var vehicleQuery = (from v in _db.Vehicles
                                select new VehicleViewDTO
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
                                });
            //Returning the result
            return await PagedList<VehicleViewDTO>.CreateAsync(vehicleQuery, pageNumber, pageSize);
        }
    }
}
