using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoTransaction.API.Controllers;
using CryptoTransaction.API.Persistence;
using CryptoTransaction.API.AppCore.Interfaces.Repository;
using CryptoTransaction.API.AppCore.EventBus.Command.Interface;
using CryptoTransaction.API.Domain;
using OnaxTools.Dto.Http;

public class TransactionControllerTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<ICommandBus> _commandBusMock;
    private readonly TransactionController _controller;

    public TransactionControllerTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _commandBusMock = new Mock<ICommandBus>();
        _controller = new TransactionController(_transactionRepositoryMock.Object, _commandBusMock.Object);
    }

    [Fact]
    public async Task GetTransactionsForAddress_ShouldReturnOkResult_WhenTransactionsExist()
    {
        // Arrange
        long blockNumber = 234562;
        string address = "0x12345678";
        string currency = "USDT";
        var resp = new GenResponse<List<WalletTransaction>>();
        var transactions = new List<WalletTransaction>
        {
            new WalletTransaction { BlockNumber = blockNumber, ReceiverAddress = address, Currency = currency, Amount = 100 },
            new WalletTransaction { BlockNumber = blockNumber, SenderAddress = address, Currency = currency, Amount = 200 }
        };
        resp.Result = transactions;
        resp.IsSuccess = true;
        _transactionRepositoryMock.Setup(repo => repo.GetTransactionsByQueryAsync(blockNumber, address, currency))
                                  .ReturnsAsync(resp);

        // Act
        var result = await _controller.GetTransactionsForAddress(blockNumber, address, currency);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<WalletTransaction>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetTransactionsForAddress_ShouldReturnOkResult_WhenNoTransactionsExist()
    {
        // Arrange
        long blockNumber = 234562;
        string address = "0x12345678";
        string currency = "USDT";
        var transactions = new List<WalletTransaction>();
        var resp = new GenResponse<List<WalletTransaction>>();
        resp.Result = transactions;
        resp.IsSuccess = true;
        _transactionRepositoryMock.Setup(repo => repo.GetTransactionsByQueryAsync(blockNumber, address, currency))
                                  .ReturnsAsync(resp);

        // Act
        var result = await _controller.GetTransactionsForAddress(blockNumber, address, currency);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<WalletTransaction>>(okResult.Value);
        Assert.Empty(returnValue);
    }

    [Fact]
    public async Task GetTransactionsForAddress_ShouldReturnBadRequest_WhenExceptionThrown()
    {
        // Arrange
        long blockNumber = 234562;
        string address = "0x12345678";
        string currency = "USDT";

        _transactionRepositoryMock.Setup(repo => repo.GetTransactionsByQueryAsync(blockNumber, address, currency))
                                  .ThrowsAsync(new Exception("Some error"));

        // Act
        var result = await _controller.GetTransactionsForAddress(blockNumber, address, currency);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Some error", badRequestResult.Value);
    }
}
