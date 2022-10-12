using E_Wallet.Data.Data;
using E_Wallet.Data.IRepositories;
using E_Wallet.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(WalletDbContext dbContext) : base(dbContext)
        {
        }
    }
}
