using System.Transactions;
using CryptoTransaction.API.AppCore.EventBus.Command.Interface;
using CryptoTransaction.API.AppCore.EventBus.Events.Interface;
using CryptoTransaction.API.AppCore.Interfaces.Repository;
using CryptoTransaction.API.AppCore.Interfaces.Services;
using CryptoTransaction.API.Domain;
using CryptoTransaction.API.Domain.Dtos;
using Microsoft.AspNetCore.HttpOverrides;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CryptoTransaction.API.AppCore.EventBus.Handler
{
    public class ScanBlockForDepositToAddressCommandHandler : ICommandHandler<ScanBlockForDepositToAddressCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventBus _eventBus;

        public ScanBlockForDepositToAddressCommandHandler(ITransactionService transactionService, ITransactionRepository transactionRepository, IEventBus eventBus)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task HandleAsync(ScanBlockForDepositToAddressCommand command)
        {
            var allTransactions = new List<WalletTransaction>();
            var walletDetails = await _transactionService.GetTransactionsByBlockNumberAsync(command.BlockNumber, command.Network);

            var allsortedwalletAddress = walletDetails
                .SelectMany(item => new[] { item.ReceiverAddress, item.SenderAddress })
                .Distinct() // Ensure the addresses are unique
                .ToList();
            
            for (int i = 0; i < allsortedwalletAddress.Count; i++)
            {
                if (allsortedwalletAddress[i] != null)
                {
                    var processedTransactionHashes = new HashSet<string>();

                    foreach (var address in allsortedwalletAddress)
                    {
                        var transactions = await _transactionService.GetTransactionsByWalletAddressAsync(address);

                        foreach (var tx in transactions)
                        {
                            if (!string.IsNullOrEmpty(tx.SenderAddress) &&
                                !string.IsNullOrEmpty(tx.ReceiverAddress) &&
                                !string.IsNullOrEmpty(tx.TransactionHash) &&
                                !processedTransactionHashes.Contains(tx.TransactionHash))
                            {
                                var transaction = new WalletTransaction()
                                {
                                    ReceiverAddress = tx.ReceiverAddress,
                                    SenderAddress = tx.SenderAddress,
                                    TransactionHash = tx.TransactionHash,
                                    Amount = tx.Amount,
                                    BlockNumber = tx.BlockNumber,
                                    Timestamp = DateTime.UtcNow,
                                    Currency = tx.Currency,
                                    Network = command.Network
                                };

                                allTransactions.Add(transaction);
                                processedTransactionHashes.Add(tx.TransactionHash);

                                var request = new TransactionReceivedEvent()
                                {
                                    ReceiverAddress = tx.ReceiverAddress,
                                    Amount = tx.Amount,
                                    BlockNumber = tx.BlockNumber,
                                    Currency = tx.Currency,
                                    SenderAddress = tx.SenderAddress,
                                    TransactionHash = tx.TransactionHash
                                };

                                _eventBus.Publish("transaction-topic", request);
                            }
                        }
                    }

                }
                else {
                  //Do nothing
                }
                
            }
            
            if (allTransactions.Any())
            {
                await _transactionRepository.SaveBulkTransactionAsync(allTransactions);
            }
        }
    }

}
