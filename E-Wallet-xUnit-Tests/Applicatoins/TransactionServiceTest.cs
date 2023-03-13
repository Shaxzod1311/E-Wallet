
using AutoMapper;
using E_Wallet.Data.Data;
using E_Wallet.Data.IRepositories;
using E_Wallet.Data.Repositories;
using E_Wallet.Domain.Common;
using E_Wallet.Domain.Enums;
using E_Wallet.Domain.Models;
using E_Wallet.Service.Helpers;
using E_Wallet.Service.Interfaces;
using E_Wallet.Service.Mapping;
using E_Wallet.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace E_Wallet_xUnit_Tests.Applicatoins
{
    public class TransactionServiceTest
    {
        private Mock<UnitOfWork> unitOfWork;
        private IMapper mapper;
        private IdentifyUser user;
        private ITrasnactionService transactionService;
        private Mock<HttpContextHelper> httpContextHelper;
        private Mock<IHttpContextAccessor> httpContextAccessor;
        private WalletDbContext db;



        public TransactionServiceTest()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();

            mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperConfig>()));
        }


        private void ConfigureDatabase()
        {
            var dbName = Guid.NewGuid();
            var dbOptions = new DbContextOptionsBuilder<WalletDbContext>()
                 .UseInMemoryDatabase(databaseName: dbName.ToString())
                     .Options;
            this.db = new WalletDbContext(dbOptions);
        }

        private void ConfigureClaims(Guid? userId, bool isIdentified = true)
        {
            var claims = new List<Claim>
            {

                new Claim("Id", userId != null ? userId.ToString() : null),
                new Claim("Username", userId != null ? userId.ToString() : null),
                new Claim("IsIdentified", isIdentified.ToString()),

            };

            var identity = new ClaimsIdentity(claims, "test_claims");
            var claimPrincipal = new ClaimsPrincipal(identity);
            var _moqHttpContext = new Mock<HttpContext>();

            _moqHttpContext.Setup(x => x.User).Returns(claimPrincipal);

            httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(_moqHttpContext.Object);
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimPrincipal);

            httpContextHelper = new Mock<HttpContextHelper>(httpContextAccessor.Object);

        }

        private void ConfigureServices()
        {
            user = new IdentifyUser(httpContextHelper.Object);
            unitOfWork = new Mock<UnitOfWork>(db);
            transactionService = new TransactionService(unitOfWork.Object, mapper, user);
        }

        [Fact]
        public async Task GetAllTransactionForCurrentMonth_ReturnsValidResponse_WhenTransactionsExist()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureClaims(Guid.NewGuid());
            ConfigureServices();

            var walletId = Guid.NewGuid();
            var currentMonth = DateTime.Now.Month;
            var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletId,
                Amount = 500,
                Date = DateTime.Now,
                Type = TransactionTypes.TopUp
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletId,
                Amount = 750,
                Date = DateTime.Now,
                Type = TransactionTypes.TopUp
            }
        };

            db.Transactions.AddRange(transactions);
            db.SaveChanges();

            // Act
            var result = (await transactionService.GetAllTransactionForcCurrentMonth(walletId)).Data;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(walletId, t.WalletId));
            Assert.All(result, t => Assert.Equal(currentMonth, t.Date.Month));
        }

        [Fact]
        public async Task GetAllTransactionForCurrentMonth_ThrowsException_WhenNoTransactionsExist()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureClaims(Guid.NewGuid());
            ConfigureServices();

            var walletId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<HttpStatusCodeException>(() => transactionService.GetAllTransactionForcCurrentMonth(walletId));
        }

        [Fact]
        public async Task TopUpWalletAsync_UpdatesBalance_WhenWalletExistsAndAmountIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();

            ConfigureDatabase();
            ConfigureClaims(userId);
            ConfigureServices();

            var walletId = Guid.NewGuid();
            var initialBalance = 1000m;
            var amount = 500m;
            var wallet = new Wallet
            {
                Id = walletId,
                Balance = initialBalance,
                UserId = userId
            };

            db.Wallets.Add(wallet);
            db.SaveChanges();

            // Act
            var result = await transactionService.TopUpWalletAsync(walletId, amount);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(walletId, result.Data);
            Assert.Equal(initialBalance + amount, wallet.Balance);
        }

        [Fact]
        public async Task TopUpWalletAsync_Should_ThrowException_WhenWalletNotFound()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureClaims(Guid.NewGuid());
            ConfigureServices();
            var walletId = Guid.NewGuid();
            var amount = 100;

            // Act and Assert
            await Assert.ThrowsAsync<HttpStatusCodeException>(() => transactionService.TopUpWalletAsync(walletId, amount));
        }

        [Fact]
        public async Task TopUpWalletAsync_Should_ThrowException_WhenBalanceExceedsMaxValueForUnidentifiedUser()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureClaims(Guid.NewGuid(), false);
            ConfigureServices();
            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Balance = 9000
            };
            db.Wallets.Add(wallet);
            await db.SaveChangesAsync();
            var amount = 2000;

            // Act and Assert
            await Assert.ThrowsAsync<HttpStatusCodeException>(() => transactionService.TopUpWalletAsync(wallet.Id, amount));
        }

        [Fact]
        public async Task TopUpWalletAsync_Should_ThrowException_WhenBalanceExceedsMaxValueForIdentifiedUser()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureClaims(Guid.NewGuid());
            ConfigureServices();
            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Balance = 95000
            };
            db.Wallets.Add(wallet);
            await db.SaveChangesAsync();
            var amount = 6000;

            // Act and Assert
            await Assert.ThrowsAsync<HttpStatusCodeException>(() => transactionService.TopUpWalletAsync(wallet.Id, amount));
        }

        [Fact]
        public async Task TopUpWalletAsync_Should_UpdateWalletBalance_WhenValidAmountProvided()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureClaims(Guid.NewGuid());
            ConfigureServices();

            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Balance = 10000
            };

            db.Wallets.Add(wallet);

            await db.SaveChangesAsync();
            var amount = 1000;

            // Act
            var result = await transactionService.TopUpWalletAsync(wallet.Id, amount);

            // Assert
            var updatedWallet = await db.Wallets.FirstOrDefaultAsync(w => w.Id == wallet.Id);
            Assert.NotNull(updatedWallet);
            Assert.Equal(11000M, updatedWallet.Balance);
            Assert.Equal(wallet.Id, result.Data);
        }

    }
}
