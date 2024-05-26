using CryptoTransaction.API.AppCore.Interfaces.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using CryptoTransaction.API.Common.Utils.Interface;
using CryptoTransaction.API.Domain;
using CryptoTransaction.API.Domain.Dtos;
using UserServices.Domain.DTOs;
using Microsoft.Extensions.Options;

namespace CryptoTransaction.API.AppCore.Services
{
    public class TransactionServiceImplementation : ITransactionService
    {
        private readonly IGenericApiClient _genericApiClient;
        private readonly AppSettings _appsettings;

        public TransactionServiceImplementation(IGenericApiClient genericApiClient, IOptions<AppSettings> appsettings)
        {
            _genericApiClient = genericApiClient;
            _appsettings = appsettings.Value;
        }

        public async Task<List<WalletTransactionsOnBlock>> GetTransactionsByBlockNumberAsync(long blockNumber, string network)
        {
            try
            {
                if (_appsettings.IsTest)
                {
                    var transactions = new List<WalletTransactionsOnBlock>
                    {
                         new WalletTransactionsOnBlock { ReceiverAddress = "0x87654321", SenderAddress = "0x12345678", BlockHash = "txhash1", Amount = 100, BlockNumber = 234562, Currency = "USDT" },
                         new WalletTransactionsOnBlock { ReceiverAddress = "0x98765432", SenderAddress = "0x23456789", BlockHash = "txhash2", Amount = 200, BlockNumber = 234562, Currency = "USDT" }
                    };

                    return transactions;
                }
                else
                {
                    var theblockNumber = blockNumber.ToString();

                    var url = $"{_appsettings.BaseUrl}{network}/blocks/{theblockNumber}/transactions";
                    return await _genericApiClient.GetAsync<List<WalletTransactionsOnBlock>>(url);

                }
                    
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        public async Task<List<WalletTransaction>> GetTransactionsByWalletAddressAsync(string walletAddress)
        {
            try
            {
                if (_appsettings.IsTest) 
                {
                    var transactions = new List<WalletTransaction>
                    {
                        new WalletTransaction { ReceiverAddress = "0x87654321", SenderAddress = "0x12345678", TransactionHash = "txhash1", Amount = 100, BlockNumber = 234561, Currency = "USDT" },
                        new WalletTransaction { ReceiverAddress = "0x98765432", SenderAddress = "0x23456789", TransactionHash = "txhash2", Amount = 200, BlockNumber = 234562, Currency = "USDT" }
                    };

                    return transactions;
                } else 
                {
                    var url = $"{_appsettings.BaseUrl}{walletAddress}";
                    return await _genericApiClient.GetAsync<List<WalletTransaction>>(url);
                }

               
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
    }
}
