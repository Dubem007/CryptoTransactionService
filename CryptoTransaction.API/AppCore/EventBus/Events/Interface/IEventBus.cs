using System;

namespace CryptoTransaction.API.AppCore.EventBus.Events.Interface
{
    public interface IEventBus
    {
        void Publish<TEvent>(string topic, TEvent @event) where TEvent : IEvent;
        void Subscribe<TEvent, TEventHandler>(string topic) where TEvent : IEvent where TEventHandler : IEventHandler<TEvent>;
    }
}
