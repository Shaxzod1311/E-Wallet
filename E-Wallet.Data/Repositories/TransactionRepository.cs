using E_Wallet.Data.Data;
using E_Wallet.Data.IRepositories;
using E_Wallet.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Data.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(WalletDbContext dbContext) : base(dbContext)
        {
        }
    }
}
