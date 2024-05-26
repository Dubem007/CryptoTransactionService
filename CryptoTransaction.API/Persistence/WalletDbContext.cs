using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CryptoTransaction.API.Domain;

namespace CryptoTransaction.API.Persistence
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
        }

        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<WalletNetworkRecord> WalletNetworkRecords { get; set; }
        
    }
}
