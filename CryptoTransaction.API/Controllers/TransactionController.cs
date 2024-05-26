using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CryptoTransaction.API.AppCore.Interfaces.Repository;
using CryptoTransaction.API.Domain.Dtos;
using CryptoTransaction.API.AppCore.EventBus.Command.Interface;

namespace CryptoTransaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICommandBus _commandBus;

        public TransactionController(ITransactionRepository transactionRepository, ICommandBus commandBus)
        {
            _transactionRepository = transactionRepository;
            _commandBus = commandBus;
        }

        [HttpGet("block/{blockNumber}/address/{address}/currency/{currency}")]
        public async Task<IActionResult> GetTransactionsForAddress(long blockNumber, string address, string currency)
        {
            try
            {
                // Assuming you have a way to filter transactions by block number, address, and currency
                var transactions = await _transactionRepository.GetTransactionsByQueryAsync(blockNumber, address, currency);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message);
            }
           
        }

        [HttpPost("scan-block")]
        public async Task<IActionResult> ScanBlock([FromBody] ScanBlockForDepositToAddressCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid command.");
            }

            await _commandBus.SendAsync(command);
            return Ok("Block scanning initiated.");
        }
    }
}
