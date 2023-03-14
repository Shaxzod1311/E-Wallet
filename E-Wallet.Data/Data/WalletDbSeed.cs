using E_Wallet.Domain.Enums;
using E_Wallet.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Data.Data
{
    public class WalletDbSeed
    {
        public WalletDbContext db { get; set; }

        public WalletDbSeed(WalletDbContext db)
        {
            this.db = db;


            if (db.Users.Any() || db.Wallets.Any() || db.Transactions.Any())
            {
                return;
            }


            var user1 = new User { Id = Guid.Parse("2ba5597b-5a8b-45c3-a838-c886a4873acf"), Username = "john", Password = "pass123", Name = "John Smith", Phone = "+1234567890", IsIdentified = true, SecretKey = "1KpT1XxtIm+PYLn98xbFtAmrw/ihD7QrBImsA/WFdjU=" };
            var user2 = new User { Username = "jane", Password = "pass456", Name = "Jane Doe", Phone = "+0987654321", IsIdentified = false };

            db.Users.AddRange(user1, user2);
            db.SaveChanges();

            var wallet1 = new Wallet { Id = Guid.Parse("9d887904-794d-4535-924c-fe44696bcaec"), UserId = user1.Id, Balance = 10000 };
            var wallet2 = new Wallet { UserId = user1.Id, Balance = 5000 };
            var wallet3 = new Wallet { UserId = user2.Id, Balance = 500 };

            db.Wallets.AddRange(wallet1, wallet2, wallet3);


            var transaction1 = new Transaction { UserId = user1.Id, WalletId = wallet1.Id, Amount = 5000, Date = DateTime.UtcNow, Type = TransactionTypes.TopUp };
            var transaction2 = new Transaction { UserId = user1.Id, WalletId = wallet2.Id, Amount = 1000, Date = DateTime.UtcNow, Type = TransactionTypes.TopUp };
            var transaction3 = new Transaction { UserId = user2.Id, WalletId = wallet3.Id, Amount = 200, Date = DateTime.UtcNow, Type = TransactionTypes.TopUp };

            db.Transactions.AddRange(transaction1, transaction2, transaction3);
            db.SaveChanges();
        }


    }
}
