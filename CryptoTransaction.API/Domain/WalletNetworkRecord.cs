using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CryptoTransaction.API.Common;

namespace CryptoTransaction.API.Domain
{
    public class WalletNetworkRecord: CommonProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string WalletAddress { get; set; }
        public string Currency { get; set; }
        public long BlockNumber { get; set; }
        public string Network { get; set; }
    }
}
