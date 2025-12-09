using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Customer;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreationDTO customer)
        {
            var result = await _customerService.CreateCustomer(customer);

            if(result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }

        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer([FromQuery] Guid customerID, [FromBody] CustomerUpdateDTO customer)
        {
            var result = await _customerService.UpdateCustomer(customerID, customer);

            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });

        }

        [HttpDelete("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer([FromQuery] Guid customerID)
        {
            var result = await _customerService.DeleteCustomer(customerID);

            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });

        }
        [HttpPost("BuyVehicle")]
        public async Task<IActionResult> BuyVehicle([FromQuery] Guid customerID, Guid vehicleID)
        {
            var result = await _customerService.BuyVehicle(customerID, vehicleID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpPost("SellVehicle")]
        public async Task<IActionResult> SellVehicle([FromQuery] Guid customerID, Guid vehicleID)
        {
            var result = await _customerService.SellVehicle(customerID, vehicleID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser([FromQuery] Guid customerID)
        {
            var result = await _customerService.GetCustomer(customerID);

            return Ok(result);
        }
        [HttpGet("GetBoughtVehicles")]
        public async Task<IActionResult> GetUserVehicles([FromQuery] Guid customerID)
        {
            var result = await _customerService.GetBoughtVehicles(customerID);
            return Ok(result);
        }
        [HttpGet("GetSoldVehicles")]
        public async Task<IActionResult> GetSoldVehicles([FromQuery] Guid customerID)
        {
            var result = await _customerService.GetSoldVehicles(customerID);
            return Ok(result);
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _customerService.GetCustomers();

            return Ok(result);
        }
    }
}
