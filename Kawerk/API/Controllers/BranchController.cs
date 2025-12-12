using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Branch;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    [Route("api/v1/branch")]
    public class BranchController : Controller
    {
        private readonly IBranchSevice _branchService;
        public BranchController(IBranchSevice branchService)
        {
            _branchService = branchService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateBranch([FromBody]BranchCreationDTO branch)
        {
            var result = await _branchService.CreateBranch(branch);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpPut("update/{branchID}")]
        public async Task<IActionResult> UpdateBranch([FromRoute]Guid branchID,[FromBody] BranchUpdateDTO branch)
        {
            var result = await _branchService.UpdateBranch(branchID, branch);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpDelete("delete/{branchID}")]
        public async Task<IActionResult> DeleteBranch([FromRoute]Guid branchID)
        {
            var result = await  _branchService.DeleteBranch(branchID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpPost("add-salesman/{branchID}/{salesmanID}")]
        public async Task<IActionResult> AddSalesmanToBranch([FromRoute]Guid branchID, [FromRoute]Guid salesmanID)
        {
            var result = await _branchService.AddSalesman(branchID, salesmanID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpDelete("remove-salesman/{branchID}/{salesmanID}")]
        public async Task<IActionResult> RemoveSalesmanFromBranch([FromRoute]Guid branchID, [FromRoute]Guid salesmanID)
        {
            var result = await _branchService.RemoveSalesman(branchID, salesmanID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpGet("get/{branchID}")]
        public async Task<IActionResult> GetBranch([FromRoute]Guid branchID)
        {
            var result = await _branchService.GetBranch(branchID);
            if (result == null)
                return BadRequest(new { message = "Branch not found" });
            else
                return Ok(result);
        }
        [HttpGet("get-salesmen/{branchID}")]
        public async Task<IActionResult> GetBranchSalesmen([FromRoute] Guid branchID)
        {
            var result = await _branchService.GetBranchSalesmen(branchID);
            if (result == null)
                return BadRequest(new { message = "No salesmen in this branch" });
            else
                return Ok(result);
        }
        [HttpGet("get")]
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
