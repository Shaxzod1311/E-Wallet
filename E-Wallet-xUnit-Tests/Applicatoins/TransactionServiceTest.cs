
using AutoMapper;
using E_Wallet.Data.Data;
using E_Wallet.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace E_Wallet_xUnit_Tests.Applicatoins
{
    public class TransactionServiceTest
    {
        private readonly TransactionService transactionService;
        private readonly Mapper mapper;

        private WalletDbContext db;

        private void ConfigureDatabase()
        {
            var dbName = Guid.NewGuid();
            var dbOptions = new DbContextOptionsBuilder<WalletDbContext>()
                 .UseInMemoryDatabase(databaseName: dbName.ToString())
                     .Options;
            this.db = new WalletDbContext(dbOptions);


        }
    }
}
