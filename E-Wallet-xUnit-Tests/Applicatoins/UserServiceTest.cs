using AutoMapper;
using E_Wallet.Data.Data;
using E_Wallet.Data.Repositories;
using E_Wallet.Domain.Common;
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
    public class UserServiceTest
    {
        private IMapper mapper;
        private IdentifyUser user;
        private Mock<IHttpContextAccessor> moqHttpContextAccessor;
        private Mock<HttpContextHelper> httpContextHelper;
        private WalletDbContext db;
        private UserService userService;
        private Mock<UnitOfWork> moqUnitOfWrok;

        public UserServiceTest()
        {
            moqHttpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextHelper = new Mock<HttpContextHelper>(moqHttpContextAccessor.Object);

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperConfig>();
            });

            mapper = configurationProvider.CreateMapper();

        }

        private void ConfigureClaims(Guid? userId)
        {
            var claims = new List<Claim>
            {

                new Claim("UserId", userId != null ? userId.ToString() : null),
            };

            var identity = new ClaimsIdentity(claims, "test_claims");
            var claimPrincipal = new ClaimsPrincipal(identity);
            var _moqHttpContext = new Mock<HttpContext>();

            _moqHttpContext.Setup(x => x.User).Returns(claimPrincipal);

            moqHttpContextAccessor = new Mock<IHttpContextAccessor>();
            moqHttpContextAccessor.Setup(x => x.HttpContext).Returns(_moqHttpContext.Object);
            httpContextHelper.Setup(x => x.Context.User).Returns(claimPrincipal);


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
            user = new IdentifyUser(httpContextHelper.Object);
            userService = new UserService(moqUnitOfWrok.Object, mapper, user);
        }

        [Fact]
        public async Task CheckAccountExistsAsync_ReturnsValidResponse_WhenWalletExists()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureClaims(Guid.NewGuid());
            ConfigureServices();

            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Balance = 1000
            };

            db.Wallets.Add(wallet);
            await db.SaveChangesAsync();

            // Act
            var result = await userService.CheckAccountExistsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(wallet.Id, result.Data.Id);
            Assert.Equal(wallet.UserId, result.Data.UserId);
            Assert.Equal(wallet.Balance, result.Data.Balance);
        }


        [Fact]
        public async Task CheckAccountExistsAsync_Throws404Error_WhenWalletDoesNotExist()
        {
            // Arrange
            ConfigureDatabase();
            ConfigureClaims(Guid.NewGuid());
            ConfigureServices();

            // Act & Assert
            await Assert.ThrowsAsync<HttpStatusCodeException>(() => userService.CheckAccountExistsAsync());

        }

    }
}

