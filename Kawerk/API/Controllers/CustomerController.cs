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

            if (result == 4)
                return Ok(new { message = "Customer Created Succesfully" });
            else if (result == 3)
                return BadRequest(new { message = "Customer already Exists" });
            else if (result == 2)
                return BadRequest(new { message = "Invalid Password" });
            else if (result == 1)
                return BadRequest(new { message = "Invalid Email" });
            else
                return BadRequest(new { message = "Invalid DTO" });
        }

        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer([FromQuery] Guid customerID, [FromBody] CustomerUpdateDTO customer)
        {
            var result = await _customerService.UpdateCustomer(customerID, customer);

            if (result == 3)
                return Ok(new { message = "Updated Successfully" });
            else if (result == 2)
                return BadRequest(new { message = "Username already in use" });
            else if (result == 1)
                return BadRequest(new { message = "Customer does not exist" });
            else
                return BadRequest(new { message = "Invalid DTO" });

        }

        [HttpDelete("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer([FromQuery] Guid customerID)
        {
            var result = await _customerService.DeleteCustomer(customerID);

            if (result == 2)
                return Ok(new { message = "Customer Deleted Successfully" });
            else if (result == 1)
                return BadRequest(new { message = "Customer does not exist" });
            else
                return BadRequest(new { message = "Invalid DTO" });

        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser([FromQuery] Guid customerID)
        {
            var result = await _customerService.GetCustomer(customerID);

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
