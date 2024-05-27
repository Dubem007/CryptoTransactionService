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
            var processedTransactionHashes = new HashSet<string>();
            var walletDetails = await _transactionService.GetTransactionsByBlockNumberAsync(command.BlockNumber, command.Network);

            var allsortedwalletAddress = walletDetails
                .SelectMany(item => new[] { item.ReceiverAddress, item.SenderAddress })
                .Distinct() // Ensure the addresses are unique
                .ToList();
            
            for (int i = 0; i < allsortedwalletAddress.Count; i++)
            {
                if (allsortedwalletAddress[i] != null)
                {
                    var transactions = await _transactionService.GetTransactionsByWalletAddressAsync(allsortedwalletAddress[i]);
                   

                    for (int k = 0; k < transactions.Count; k++)
                    {
                        if (!string.IsNullOrEmpty(transactions[k].SenderAddress) &&
                            !string.IsNullOrEmpty(transactions[k].ReceiverAddress) &&
                            !string.IsNullOrEmpty(transactions[k].TransactionHash) &&
                            !processedTransactionHashes.Contains(transactions[k].TransactionHash))
                        {
                            var transaction = new WalletTransaction()
                            {
                                ReceiverAddress = transactions[k].ReceiverAddress,
                                SenderAddress = transactions[k].SenderAddress,
                                TransactionHash = transactions[k].TransactionHash,
                                Amount = transactions[k].Amount,
                                BlockNumber = transactions[k].BlockNumber,
                                Timestamp = DateTime.UtcNow,
                                Currency = transactions[k].Currency,
                                Network = command.Network
                            };

                            allTransactions.Add(transaction);
                            processedTransactionHashes.Add(transactions[k].TransactionHash);

                            var request = new TransactionReceivedEvent()
                            {
                                ReceiverAddress = transactions[k].ReceiverAddress,
                                Amount = transactions[k].Amount,
                                BlockNumber = transactions[k].BlockNumber,
                                Currency = transactions[k].Currency,
                                SenderAddress = transactions[k].SenderAddress,
                                TransactionHash = transactions[k].TransactionHash
                            };

                            _eventBus.Publish("transaction-topic", request);
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
