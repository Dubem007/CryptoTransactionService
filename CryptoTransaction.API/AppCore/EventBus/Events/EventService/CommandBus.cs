using CryptoTransaction.API.AppCore.EventBus.Command.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoTransaction.API.AppCore.EventBus.Events.EventService
{
    public class CommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            //var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
            //if (handler == null)
            //{
            //    throw new InvalidOperationException($"Command handler for '{typeof(TCommand).Name}' not registered.");
            //}
            //await handler.HandleAsync(command);

            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
                if (handler == null)
                {
                    throw new InvalidOperationException($"Command handler for '{typeof(TCommand).Name}' not registered.");
                }
                await handler.HandleAsync(command);
            }
        }
    }
}
