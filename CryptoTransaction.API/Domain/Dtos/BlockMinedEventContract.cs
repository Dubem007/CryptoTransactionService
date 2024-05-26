using CryptoTransaction.API.AppCore.EventBus.Events.Interface;

namespace CryptoTransaction.API.Domain.Dtos
{
    public class BlockMinedEventContract : IEvent
    {
        public BlockMinedEventContract(long blockNumber, string walletAddress, string network, string currency)
        {
            BlockNumber = blockNumber;
            WalletAddress = walletAddress;
            Network = network;
            Currency = currency;
        }

        public long BlockNumber { get; }
        public string WalletAddress { get; }
        public string Network { get; }
        public string Currency { get; }
    }
}
