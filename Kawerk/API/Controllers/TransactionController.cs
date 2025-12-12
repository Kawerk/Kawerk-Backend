using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    [Route("api/vi/transaction")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        //        *********** Setters ***********
        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreationDTO transaction)
        {
            var result = await _transactionService.CreateTransaction(transaction);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpDelete("delete/{transactionID}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid transactionID)
        {
            var result = await _transactionService.DeleteTransaction(transactionID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        //        *********** Getters ***********
        [HttpGet("get/{transactionID}")]
        public async Task<IActionResult> GetTransaction([FromRoute] Guid transactionID)
        {
            var result = await _transactionService.GetTransaction(transactionID);
            return Ok(result);
        }
        [HttpGet("get")]
        public async Task<IActionResult> GetTransactions()
        {
            var result = await _transactionService.GetTransactions();
            return Ok(result);
        }
    }
}
