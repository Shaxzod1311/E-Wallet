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
    public class IncomeRepository : GenericRepository<Income>, IIncomeRepository
    {
        public IncomeRepository(WalletDbContext dbContext) : base(dbContext)
        {
        }
    }
}
