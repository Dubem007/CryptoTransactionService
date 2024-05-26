using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CryptoTransaction.API.Common;

namespace CryptoTransaction.API.Domain
{
    public class WalletTransaction: CommonProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TransactionHash { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal Amount { get; set; }
        public string Network { get; set; }
        public string Currency { get; set; }
        public long BlockNumber { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
