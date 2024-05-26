using CryptoTransaction.API.AppCore.EventBus.Events.Interface;

namespace CryptoTransaction.API.Domain.Dtos
{
    public class TransactionReceivedEvent : IEvent
    {
        public long BlockNumber { get; set; }
        public string ReceiverAddress { get; set; }
        public string SenderAddress { get; set; }
        public string TransactionHash { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }

}
