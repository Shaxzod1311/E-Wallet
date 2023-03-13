using E_Wallet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Wallet.Data.Data
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) :
            base(options)
        { }

        public DbSet<User>? Users { get; set; }
        public DbSet<Wallet>? Wallets { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }
    }
}