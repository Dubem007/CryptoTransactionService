using CryptoTransaction.API.AppCore.EventBus.Command.Interface;
using CryptoTransaction.API.AppCore.EventBus.Events.Interface;
using CryptoTransaction.API.Domain.Dtos;
using CryptoTransaction.API.Persistence;
using Microsoft.AspNetCore.HttpOverrides;

namespace CryptoTransaction.API.AppCore.EventBus.Handler
{
    public class BlockMinedEventHandler : IEventHandler<BlockMinedEvent>
    {
        private readonly ICommandBus _commandBus;
        private readonly IEventBus _eventBus;
        private readonly WalletDbContext _context;


        public BlockMinedEventHandler(ICommandBus commandBus, IEventBus eventBus, WalletDbContext context)
        {
            _commandBus = commandBus;
            _eventBus = eventBus;
            _eventBus.Subscribe<BlockMinedEvent, BlockMinedEventHandler>("transaction-topic");
        }
        public async Task HandleAsync(BlockMinedEvent @event)
        {
            var request = new ScanBlockForDepositToAddressCommand(){
                               BlockNumber = @event.BlockNumber,
                               Network =  @event.Network,
                               BlockHash = @event.BlockHash
                            };
            //Send a scan block comand to get wallet transactions from wallet address
            await _commandBus.SendAsync(request);
        }
    }
}
