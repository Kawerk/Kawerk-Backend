using Kawerk.Application.Interfaces;
using Kawerk.Infastructure.DTOs.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace Kawerk.API.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        //        *********** Setters ***********
        [HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreationDTO transaction)
        {
            var result = await _transactionService.CreateTransaction(transaction);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        [HttpDelete("DeleteTransaction/{transactionID}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid transactionID)
        {
            var result = await _transactionService.DeleteTransaction(transactionID);
            if (result.status == 0)
                return BadRequest(new { message = result.msg });
            else
                return Ok(new { message = result.msg });
        }
        //        *********** Getters ***********
        [HttpGet("GetTransaction/{transactionID}")]
        public async Task<IActionResult> GetTransaction([FromRoute] Guid transactionID)
        {
            var result = await _transactionService.GetTransaction(transactionID);
            return Ok(result);
        }
        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var result = await _transactionService.GetTransactions();
            return Ok(result);
        }
    }
}
