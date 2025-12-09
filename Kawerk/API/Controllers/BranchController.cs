using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Branch;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranchSevice _branchService;
        public BranchController(IBranchSevice branchService)
        {
            _branchService = branchService;
        }
        [HttpPost("CreateBranch")]
        public async Task<IActionResult> CreateBranch([FromBody]BranchCreationDTO branch)
        {
            var result = await _branchService.CreateBranch(branch);
            if (result == 2)
                return Ok(new { message = "Branch created Succesfully" });
            else if (result == 1)
                return BadRequest(new { message = "Name or Location already in use" });
            else
                return BadRequest(new { message = "Faulty DTO given" });
        }
        [HttpPut("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch([FromQuery]Guid branchID,[FromBody] BranchUpdateDTO branch)
        {
            var result = await _branchService.UpdateBranch(branchID, branch);
            if (result == 4)
                return Ok(new { message = "Branch updated Succesfully" });
           else if (result == 3)
                return BadRequest(new { message = "Location already in use" });
            else if (result == 2)
                return BadRequest(new { message = "Name already in use" });
            else if (result == 1)
                return BadRequest(new { message = "Branch not found" });
            else
                return BadRequest(new { message = "Faulty DTO given" });
        }
        [HttpDelete("DeleteBranch")]
        public async Task<IActionResult> DeleteBranch([FromQuery]Guid branchID)
        {
            var result = await  _branchService.DeleteBranch(branchID);
            if (result == 2)
                return Ok(new { message = "Branch deleted Succesfully" });
            else if (result == 1)
                return BadRequest(new { message = "Branch not found" });
            else
                return BadRequest(new { message = "Faulty DTO given" });
        }
        [HttpGet("GetBranch")]
        public async Task<IActionResult> GetBranch([FromQuery]Guid branchID)
        {
            var result = await _branchService.GetBranch(branchID);
            if (result == null)
                return BadRequest(new { message = "Branch not found" });
            else
                return Ok(result);
        }
        [HttpGet("GetBrances")]
        public async Task<IActionResult> GetBranches()
        {
            var result = await _branchService.GetBranches();
            if (result == null)
                return BadRequest(new { message = "No branches in database" });
            else
                return Ok(result);
        }
    }
}
