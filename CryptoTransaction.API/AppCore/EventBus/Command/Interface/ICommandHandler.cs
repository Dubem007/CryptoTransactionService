using CryptoTransaction.API.Domain.Dtos;

namespace CryptoTransaction.API.AppCore.EventBus.Command.Interface
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
