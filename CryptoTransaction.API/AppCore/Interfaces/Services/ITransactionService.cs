using System.Transactions;
using CryptoTransaction.API.Domain;
using CryptoTransaction.API.Domain.Dtos;

namespace CryptoTransaction.API.AppCore.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<List<WalletTransactionsOnBlock>> GetTransactionsByBlockNumberAsync(long blockNumber, string network);
        Task<List<WalletTransaction>> GetTransactionsByWalletAddressAsync(string walletAddress);
    }
}
