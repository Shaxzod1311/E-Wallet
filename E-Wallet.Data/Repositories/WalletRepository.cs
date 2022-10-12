using E_Wallet.Data.Data;
using E_Wallet.Data.IRepositories;
using E_Wallet.Domain.Models;

namespace E_Wallet.Data.Repositories
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(WalletDbContext dbContext) : base(dbContext)
        {
        }
    }
}
