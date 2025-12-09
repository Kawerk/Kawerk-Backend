using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Salesman;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    [Route("api/[controller]")]
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

            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpPut("UpdateSalesman")]
        public async Task<IActionResult> UpdateSalesman([FromQuery] Guid salesmanID, [FromBody] SalesmanUpdateDTO salesman)
        {
            var result = await _salesmanService.UpdateSalesman(salesmanID, salesman);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpDelete("DeleteSalesman")]
        public async Task<IActionResult> DeleteSalesman([FromQuery] Guid salesmanID)
        {
            var result = await _salesmanService.DeleteSalesman(salesmanID);

            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
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
