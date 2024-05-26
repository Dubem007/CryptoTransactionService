namespace CryptoTransaction.API.Domain.Dtos
{
    public class WalletTransactionsOnBlock
    {
        public string BlockHash { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public long BlockNumber { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
