using Microsoft.Extensions.Options;
using System.ComponentModel.Design;
using System;
using CryptoTransaction.API.AppCore.Repository;
using CryptoTransaction.API.AppCore.Interfaces.Repository;
using CryptoTransaction.API.AppCore.EventBus.Events.Interface;
using CryptoTransaction.API.AppCore.EventBus.Command.Interface;
using CryptoTransaction.API.AppCore.Interfaces.Services;
using CryptoTransaction.API.AppCore.Services;
using CryptoTransaction.API.Domain.Dtos;
using CryptoTransaction.API.AppCore.EventBus.Handler;
using CryptoTransaction.API.AppCore.EventBus.Events.EventService;
using CryptoTransaction.API.Common.Utils.Clients;
using CryptoTransaction.API.Common.Utils.Interface;
using UserServices.Domain.DTOs;

namespace CryptoTransaction.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection CustomDependencyInjection(this IServiceCollection services,
        IConfiguration configuration, WebApplicationBuilder builder)
        {
            services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
            services.AddScoped<IEventBus, EventBus>();
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IEventHandler<BlockMinedEvent>, BlockMinedEventHandler>();

            // Register command bus and handlers
            services.AddScoped<ICommandHandler<ScanBlockForDepositToAddressCommand>, ScanBlockForDepositToAddressCommandHandler>();

            services.AddSingleton<IGenericApiClient, GenericApiClient>();
            // Register other services
            services.AddHttpClient(); // Add HttpClient factory
            services.AddScoped<ITransactionService, TransactionServiceImplementation>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            

           

            return services;
        }
    }
}
