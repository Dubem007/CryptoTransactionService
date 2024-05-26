using CryptoTransaction.API.Domain.Dtos;

namespace CryptoTransaction.API.AppCore.EventBus.Command.Interface
{
    public interface ICommandBus
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
