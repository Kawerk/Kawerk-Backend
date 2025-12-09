using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Salesman;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    public class SalesmanController : Controller
    {
        private readonly ISalesmanService _salesmanService;
        public SalesmanController(ISalesmanService salesmanService)
        {
            _salesmanService = salesmanService;
        }
        [HttpPost("CreateSalesman")]
        public async Task<IActionResult> CreateSalesman([FromBody] SalesmanCreationDTO salesman)
        {
            var result = await _salesmanService.CreateSalesman(salesman);

            if (result == 4)
                return Ok(new { message = "Salesman created Succesfully" });
            else if (result == 3)
                return BadRequest(new { message = "Branch not found" });
            else if (result == 2)
                return BadRequest(new { message = "Invalid Password" });
            else if (result == 1)
                return BadRequest(new { message = "Invalid Email" });
            else
                return BadRequest(new { message = "Faulty DTO" });
        }
        [HttpPut("UpdateSalesman")]
        public async Task<IActionResult> UpdateSalesman([FromQuery] Guid salesmanID, [FromBody] SalesmanUpdateDTO salesman)
        {
            var result = await _salesmanService.UpdateSalesman(salesmanID, salesman);
            if (result == 2)
                return Ok(new { message = "Saleman Updated Successfully" });
            else if (result == 1)
                return BadRequest(new { message = "Saleman not found" });
            else
                return BadRequest(new { message = "Faulty DTO" });
        }
        [HttpDelete("DeleteSalesman")]
        public async Task<IActionResult> DeleteSalesman([FromQuery] Guid salesmanID)
        {
            var result = await _salesmanService.DeleteSalesman(salesmanID);

            if (result == 2)
                return Ok(new { message = "Salesman Deleted Succesfully" });
            else if (result == 1)
                return BadRequest(new { message = "Saleman not found" });
            else
                return BadRequest(new { message = "Faulty DTO" });
        }
        [HttpGet("GetSalesman")]
        public async Task<IActionResult> GetSalesman([FromQuery]Guid salesmanID)
        {
            var result = await _salesmanService.GetSalesman(salesmanID);
            if (result == null)
                return BadRequest(new { message = "Salesman not found" });
            else
                return Ok(result);
        }
        [HttpGet("GetSalesmen")]
        public async Task<IActionResult> GetSalesmen()
        {
            var result = await _salesmanService.GetSalesmen();
            if (result == null)
                return BadRequest(new { message = "Salesman not found" });
            else
                return Ok(result);
        }
    }
}
