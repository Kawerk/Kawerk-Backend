using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Manufacturer;
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
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpPut("UpdateManufacturer")]
        public async Task<IActionResult> UpdateManufacturer([FromQuery]Guid manufacturerGuid, [FromBody] ManufacturerUpdateDTO manufacturer)
        {
            var result = await _manufacturerService.UpdateManufacturer(manufacturerGuid, manufacturer);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpDelete("DeleteManufacturer")]
        public async Task<IActionResult> DeleteManufacturer([FromQuery]Guid manufacturerID)
        {
            var result = await _manufacturerService.DeleteManufacturer(manufacturerID);

            if(result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
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
