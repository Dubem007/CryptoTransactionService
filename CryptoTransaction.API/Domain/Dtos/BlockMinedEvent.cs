using CryptoTransaction.API.AppCore.EventBus.Events.Interface;

namespace CryptoTransaction.API.Domain.Dtos
{
    public class BlockMinedEvent : IEvent
    {
        public long BlockNumber { get; set; }
        public string Network { get; set; }
        public string BlockHash { get; set; }
    }

}
