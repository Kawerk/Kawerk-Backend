using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("CreateVehicle")]
        public async Task<IActionResult> CreateVehicle([FromBody]VehicleDTO vehicle)
        {
            var result = await _vehicleService.CreateVehicle(vehicle);
            if (result == 1)
                return Ok(new { message = "Vehicle created Succesfully" });
            else
                return BadRequest(new { message = "Faulty DTO given" });
        }
        [HttpPut("UpdateVehicle")]
        public async Task<IActionResult> UpdateVehicle([FromQuery]Guid vehicleID,[FromBody]VehicleDTO vehicle)
        {
            var result = await _vehicleService.UpdateVehicle(vehicleID, vehicle);

            if (result == 2)
                return Ok(new { message = "Vehicle Updated Succesfully" });
            else if (result == 1)
                return BadRequest(new { message = "Vehicle not found" });
            else
                return BadRequest(new { message = "Faulty DTO given" });
        }
        [HttpDelete("DeleteVehicle")]
        public async Task<IActionResult> DeleteVehicle([FromQuery] Guid vehicleID)
        {
            var result = await _vehicleService.DeleteVehicle(vehicleID);

            if (result == 2)
                return Ok(new { message = "Vehicle deleted Succesfully" });
            else if (result == 1)
                return BadRequest(new { message = "Vehicle not found" });
            else
                return BadRequest(new { message = "Faulty DTO given" });
        }
        [HttpGet("GetVehicle")]
        public async Task<IActionResult> GetVehicle([FromQuery] Guid vehicleID)
        {
            var result = await _vehicleService.GetVehicle(vehicleID);

            return Ok(result);
        }
        [HttpGet("GetVehicles")]
        public async Task<IActionResult> GetVehicles()
        {
            var result = await _vehicleService.GetVehicles();

            return Ok(result);
        }

    }
}
