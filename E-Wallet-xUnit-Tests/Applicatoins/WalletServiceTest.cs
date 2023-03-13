using AutoMapper;
using E_Wallet.Data.Data;
using E_Wallet.Data.Repositories;
using E_Wallet.Domain.Models;
using E_Wallet.Service.Helpers;
using E_Wallet.Service.Mapping;
using E_Wallet.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_xUnit_Tests.Applicatoins
{
    public class WalletServiceTest
    {
        private IMapper mapper;
        private WalletDbContext db;
        private WalletService walletService;
        private Mock<UnitOfWork> moqUnitOfWrok;

        public WalletServiceTest()
        {

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperConfig>();
            });

            mapper = configurationProvider.CreateMapper();

        }

        private void ConfigureDatabase()
        {
            var dbName = Guid.NewGuid();
            var dbOptions = new DbContextOptionsBuilder<WalletDbContext>()
                 .UseInMemoryDatabase(databaseName: dbName.ToString())
                     .Options;
            this.db = new WalletDbContext(dbOptions);


        }

        private void ConfigureServices()
        {
            moqUnitOfWrok = new Mock<UnitOfWork>(db);
            walletService = new WalletService(moqUnitOfWrok.Object, mapper);
        }


        [Fact]
        public async Task GetAccountBalanceAsync_ReturnsValidResponse_WhenAccountExists()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureServices();

            var account = new Wallet
            {
                Id = Guid.NewGuid(),
                Balance = 1000
            };

            db.Wallets.Add(account);
            await db.SaveChangesAsync();

            // Act
            var result = await walletService.GetAccountBalanceAsync(account.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(account.Id, result.Data.Id);
            Assert.Equal(account.UserId, result.Data.UserId);
            Assert.Equal(account.Balance, result.Data.Balance);
        }

        [Fact]
        public async Task GetAccountBalanceAsync_ReturnsEmptyResponse_WhenAccountDoesNotExist()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureServices();

            var accountId = Guid.NewGuid();

            // Act
            var result = await walletService.GetAccountBalanceAsync(accountId);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetAccountBalanceAsync_ReturnsEmptyResponse_WhenAccountIdIsInvalid()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureServices();

            var invalidAccountId = Guid.Empty;

            // Act
            var result = await walletService.GetAccountBalanceAsync(invalidAccountId);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
        }


    }
}
