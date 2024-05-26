using CryptoTransaction.API.Domain.Dtos;

namespace CryptoTransaction.API.AppCore.EventBus.Events.Interface
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
