using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerService _manufacturerService;
        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }
        [HttpPost("CreateManufacturer")]
        public async Task<IActionResult> CreateManufacturer([FromBody] ManufacturerCreationDTO manufacturer)
        {
            var result = await _manufacturerService.CreateManufacturer(manufacturer);
            if (result == 2)
                return Ok(new { message = "Manufacturer created successsfully" });
            else if (result == 1)
                return BadRequest(new { message = "Name is already in use" });
            else
                return BadRequest(new { message = "Faulty DTO given" });
        }
        [HttpPut("UpdateManufacturer")]
        public async Task<IActionResult> UpdateManufacturer([FromQuery]Guid manufacturerGuid, [FromBody] ManufacturerUpdateDTO manufacturer)
        {
            var result = await _manufacturerService.UpdateManufacturer(manufacturerGuid, manufacturer);
            if(result == 3)
                return Ok(new { message = "Manufacturer Updated successsfully" });
            else if (result == 2)
                return BadRequest(new { message = "Name is already in use" });
            else if (result == 1)
                return BadRequest(new { message = "Manufacturer not found" });
            else
                return BadRequest(new { message = "Faulty DTO given" });
        }
        [HttpDelete("DeleteManufacturer")]
        public async Task<IActionResult> DeleteManufacturer([FromQuery]Guid manufacturerID)
        {
            var result = await _manufacturerService.DeleteManufacturer(manufacturerID);

            if (result == 2)
                return Ok(new { message = "Manufacturer deleated successsfully" });
            else if (result == 1)
                return BadRequest(new { message = "Manufacturer not found" });
            else
                return BadRequest(new { message = "Faulty ID given" });
        }
        [HttpGet("GetManufacturer")]
        public async Task<IActionResult> GetManufacturer([FromQuery] Guid manufacturerID)
        {
            var result = await _manufacturerService.GetManufacturer(manufacturerID);

            if (result == null)
                return NotFound(new { message = "not found" });
            else
                return Ok(result);
        }
        [HttpGet("GetManufacturers")]
        public async Task<IActionResult> GetManufacturers()
        {
            var result = await _manufacturerService.GetManufacturers();

            if (result == null)
                return NotFound(new { message = "Empty Database" });
            else
                return Ok(result);
        }
    }
}
