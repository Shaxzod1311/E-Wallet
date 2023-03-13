using E_Wallet.Data.Data;
using E_Wallet.Data.IRepositories;

namespace E_Wallet.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WalletDbContext dbContext;

        public UnitOfWork(WalletDbContext dbContext)
        {
            this.dbContext = dbContext;

            Users = new UserRepository(dbContext);
            Transaction = new TransactionRepository(dbContext);
            Wallets = new WalletRepository(dbContext);
        }

        public IUserRepository Users { get; }

        public IWalletRepository Wallets { get; }
        public ITransactionRepository Transaction { get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}

