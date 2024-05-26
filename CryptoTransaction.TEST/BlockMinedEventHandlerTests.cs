using CryptoTransaction.API.AppCore.EventBus.Command.Interface;
using CryptoTransaction.API.AppCore.EventBus.Events.Interface;
using CryptoTransaction.API.AppCore.EventBus.Handler;
using CryptoTransaction.API.Domain.Dtos;
using CryptoTransaction.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTransaction.TEST
{
    public class BlockMinedEventHandlerTests
    {
        private readonly Mock<ICommandBus> _commandBusMock;
        private readonly Mock<IEventBus> _eventBusMock;
        private readonly WalletDbContext _context;
        private readonly BlockMinedEventHandler _handler;

        public BlockMinedEventHandlerTests()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _eventBusMock = new Mock<IEventBus>();

            var options = new DbContextOptionsBuilder<WalletDbContext>()
                .UseInMemoryDatabase(databaseName: "WalletTestDb")
                .Options;
            _context = new WalletDbContext(options);

            _eventBusMock.Setup(x => x.Subscribe<BlockMinedEvent, BlockMinedEventHandler>(It.IsAny<string>()))
                         .Verifiable();

            _handler = new BlockMinedEventHandler(_commandBusMock.Object, _eventBusMock.Object, _context);
        }

        [Fact]
        public async Task HandleAsync_ShouldSendScanBlockCommand()
        {
            // Arrange
            var blockMinedEvent = new BlockMinedEvent
            {
                BlockNumber = 234562,
                Network = "BSC",
                BlockHash = "shaasdfgh"
            };

            // Act
            await _handler.HandleAsync(blockMinedEvent);

            // Assert
            _commandBusMock.Verify(x => x.SendAsync(It.Is<ScanBlockForDepositToAddressCommand>(cmd =>
                cmd.BlockNumber == blockMinedEvent.BlockNumber &&
                cmd.Network == blockMinedEvent.Network &&
                cmd.BlockHash == blockMinedEvent.BlockHash
            )), Times.Once);

            _eventBusMock.Verify(x => x.Subscribe<BlockMinedEvent, BlockMinedEventHandler>("transaction-topic"), Times.Once);
        }
    }
}
