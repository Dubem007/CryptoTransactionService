using System.Collections.Concurrent;
using CryptoTransaction.API.AppCore.EventBus.Events.Interface;

namespace CryptoTransaction.API.AppCore.EventBus.Events.EventService
{
    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<string, Dictionary<Type, List<object>>> _handlers;

        public EventBus()
        {
            _handlers = new ConcurrentDictionary<string, Dictionary<Type, List<object>>>();
        }

        public void Publish<TEvent>(string topic, TEvent @event) where TEvent : IEvent
        {
            if (_handlers.ContainsKey(topic))
            {
                var eventType = @event.GetType();
                var handlers = _handlers[topic];

                if (handlers.ContainsKey(eventType))
                {
                    var eventHandlers = handlers[eventType];
                    Parallel.ForEach(eventHandlers, handler =>
                    {
                        if (handler is IEventHandler<TEvent> eventHandler)
                        {
                            eventHandler.HandleAsync(@event).GetAwaiter().GetResult();
                        }
                    });
                }
            }
        }

        public void Subscribe<TEvent, TEventHandler>(string topic)
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var handler = Activator.CreateInstance<TEventHandler>();

            if (!_handlers.ContainsKey(topic))
            {
                _handlers[topic] = new Dictionary<Type, List<object>>();
            }

            if (!_handlers[topic].ContainsKey(eventType))
            {
                _handlers[topic][eventType] = new List<object>();
            }

            _handlers[topic][eventType].Add(handler);
        }
    }
}
