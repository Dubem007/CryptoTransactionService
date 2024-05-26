using System.Windows.Input;
using ICommand = CryptoTransaction.API.AppCore.EventBus.Command.Interface.ICommand;

namespace CryptoTransaction.API.Domain.Dtos
{
    public class ScanBlockForDepositAddressCommand : ICommand
    {
        public ScanBlockForDepositAddressCommand(long blockNumber, string network, string walletAddress = null, string currency = null)
        {
            BlockNumber = blockNumber;
            WalletAddress = walletAddress;
            Network = network;
            Currency = currency;
        }

        public long BlockNumber { get; set; }
        public string WalletAddress { get; set; }
        public string Network { get; set; }
        public string Currency { get; set; }
    }

}
