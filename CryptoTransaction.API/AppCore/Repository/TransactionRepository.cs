using CryptoTransaction.API.AppCore.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using CryptoTransaction.API.Domain;
using CryptoTransaction.API.Persistence;
using Microsoft.AspNetCore.HttpOverrides;

namespace CryptoTransaction.API.AppCore.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly WalletDbContext _dbContext;

        public TransactionRepository(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveTransactionAsync(WalletTransaction transaction)
        {
            await _dbContext.WalletTransactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveBulkTransactionAsync(List<WalletTransaction> transaction)
        {
            await _dbContext.WalletTransactions.AddRangeAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<WalletTransaction>> GetTransactionsForAddressAsync(string walletAddress)
        {
            try
            {
                return await _dbContext.WalletTransactions
               .Where(t => t.ReceiverAddress == walletAddress || t.SenderAddress == walletAddress).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        public async Task<List<WalletNetworkRecord>> GetWalletDetailRecordAsync(long blockNumber, string network)
        {
            try
            {
                return await _dbContext.WalletNetworkRecords
                .Where(t => t.BlockNumber == blockNumber && t.Network == network).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<WalletTransaction>> GetTransactionsByQueryAsync(long blockNumber, string walletAddress, string currency)
        {
            try
            {
                return await _dbContext.WalletTransactions
                 .Where(t => t.BlockNumber == blockNumber && t.Currency == currency && t.ReceiverAddress == walletAddress || t.SenderAddress == walletAddress).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
