using CryptoTransaction.API.AppCore.EventBus.Events.Interface;
using CryptoTransaction.API.AppCore.EventBus.Handler;
using CryptoTransaction.API.AppCore.Interfaces.Repository;
using CryptoTransaction.API.AppCore.Interfaces.Services;
using CryptoTransaction.API.Domain.Dtos;
using CryptoTransaction.API.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoTransaction.TEST
{
    public class ScanBlockForDepositToAddressCommandHandlerTests
    {
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IEventBus> _eventBusMock;
        private readonly ScanBlockForDepositToAddressCommandHandler _handler;

        public ScanBlockForDepositToAddressCommandHandlerTests()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _eventBusMock = new Mock<IEventBus>();

            _handler = new ScanBlockForDepositToAddressCommandHandler(_transactionServiceMock.Object, _transactionRepositoryMock.Object, _eventBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldSaveTransactionsAndPublishEvents()
        {
            // Arrange
            var command = new ScanBlockForDepositToAddressCommand
            {
                BlockNumber = 234562,
                Network = "BSC",
                BlockHash = "shaasdfgh"
            };

            var walletDetails = new List<WalletTransactionsOnBlock>
            {
                new WalletTransactionsOnBlock { ReceiverAddress = "0x87654321", SenderAddress = "0x12345678", BlockHash = "txhash1", Amount = 100, BlockNumber = 234562, Currency = "USDT" },
                new WalletTransactionsOnBlock { ReceiverAddress = "0x98765432", SenderAddress = "0x23456789", BlockHash = "txhash2", Amount = 200, BlockNumber = 234562, Currency = "USDT" }
            };

            var transactions = new List<WalletTransaction>
            {
                new WalletTransaction { ReceiverAddress = "0x87654321", SenderAddress = "0x12345678", TransactionHash = "txhash1", Amount = 100, BlockNumber = 234562, Currency = "USDT" },
                new WalletTransaction { ReceiverAddress = "0x98765432", SenderAddress = "0x23456789", TransactionHash = "txhash2", Amount = 200, BlockNumber = 234562, Currency = "USDT" }
            };

           _transactionServiceMock.Setup(x => x.GetTransactionsByBlockNumberAsync(command.BlockNumber, command.Network))
                               .ReturnsAsync(walletDetails);

            _transactionServiceMock.Setup(x => x.GetTransactionsByWalletAddressAsync(It.IsAny<string>()))
                                 .ReturnsAsync(transactions);

            _transactionRepositoryMock.Setup(x => x.SaveBulkTransactionAsync(It.IsAny<List<WalletTransaction>>()))
            .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleAsync(command);

            // Assert
            _transactionRepositoryMock.Verify(x => x.SaveBulkTransactionAsync(It.Is<List<WalletTransaction>>(list =>
                list.Count == 8 &&
                list.Any(tx => tx.TransactionHash == "txhash1") &&
                list.Any(tx => tx.TransactionHash == "txhash2")
            )), Times.Once);

            _eventBusMock.Verify(x => x.Publish("transaction-topic", It.IsAny<TransactionReceivedEvent>()), Times.Exactly(8));
        }

    }
}
