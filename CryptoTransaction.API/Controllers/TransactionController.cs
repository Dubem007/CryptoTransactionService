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
                if (transactions.IsSuccess)
                {
                    return Ok(transactions);
                }
                else 
                {
                    return NotFound(transactions);
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message);
            }
           
        }

    }
}
