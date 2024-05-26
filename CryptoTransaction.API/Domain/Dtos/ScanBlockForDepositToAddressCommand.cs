using CryptoTransaction.API.AppCore.EventBus.Command.Interface;

namespace CryptoTransaction.API.Domain.Dtos
{
    public class ScanBlockForDepositToAddressCommand : ICommand
    {
        public long BlockNumber { get; set; }
        public string BlockHash { get; set; }
        public string Network { get; set; }
    }
}
