using System.Transactions;
using CryptoTransaction.API.Domain;
using OnaxTools.Dto.Http;

namespace CryptoTransaction.API.AppCore.Interfaces.Repository
{
    public interface ITransactionRepository
    {
        Task SaveTransactionAsync(WalletTransaction transaction);
        Task SaveBulkTransactionAsync(List<WalletTransaction> transaction);
        Task<List<WalletTransaction>> GetTransactionsForAddressAsync(string walletAddress);
        Task<List<WalletNetworkRecord>> GetWalletDetailRecordAsync(long blockNumber, string network);
        Task<GenResponse<List<WalletTransaction>>> GetTransactionsByQueryAsync(long blockNumber, string walletAddress, string currency);
    }
}
