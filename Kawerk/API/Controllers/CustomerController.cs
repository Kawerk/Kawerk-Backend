using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Customer;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    [Route("api/v1/customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreationDTO customer)
        {
            var result = await _customerService.CreateCustomer(customer);

            if(result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }

        [HttpPut("update/{customerID}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] Guid customerID, [FromBody] CustomerUpdateDTO customer)
        {
            var result = await _customerService.UpdateCustomer(customerID, customer);

            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });

        }

        [HttpDelete("delete/{customerID}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid customerID)
        {
            var result = await _customerService.DeleteCustomer(customerID);

            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });

        }

        [HttpPost("buy-vehicle/{customerID}/{vehicleID}")]
        public async Task<IActionResult> BuyVehicle([FromRoute] Guid customerID, [FromRoute] Guid vehicleID)
        {
            var result = await _customerService.BuyVehicle(customerID, vehicleID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpPost("sell-vehicle/{customerID}/{vehicleID}")]
        public async Task<IActionResult> SellVehicle([FromRoute] Guid customerID, [FromRoute] Guid vehicleID)
        {
            var result = await _customerService.SellVehicle(customerID, vehicleID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }

        [HttpGet("get/{customerID}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid customerID)
        {
            var result = await _customerService.GetCustomer(customerID);

            return Ok(result);
        }
        [HttpGet("get-bought-vehicles/{customerID}")]
        public async Task<IActionResult> GetUserVehicles([FromRoute] Guid customerID)
        {
            var result = await _customerService.GetBoughtVehicles(customerID);
            return Ok(result);
        }
        [HttpGet("get-sold-vehicles/{customerID}")]
        public async Task<IActionResult> GetSoldVehicles([FromRoute] Guid customerID)
        {
            var result = await _customerService.GetSoldVehicles(customerID);
            return Ok(result);
        }
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _customerService.GetCustomers();

            return Ok(result);
        }
    }
}
