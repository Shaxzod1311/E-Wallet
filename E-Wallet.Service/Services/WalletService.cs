using AutoMapper;
using E_Wallet.Data.IRepositories;
using E_Wallet.Domain.Common;
using E_Wallet.Domain.Models;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Interfaces;



namespace E_Wallet.Service.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork? unitOfWork;
        public readonly IMapper mapper;

        public WalletService(IUnitOfWork? unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<WalletDTO> CheckAccountExistsAsync(Guid accountNumber)
        {
            var wallet = await unitOfWork.Wallets.GetAsync(wallet => wallet.Id == accountNumber);

            if (wallet != null)
            {
                var walletDTO = mapper.Map<WalletDTO>(wallet);
                return walletDTO;
            }
            else
                return null;
        }

        public async Task<bool> ToUpAccountAsync(Guid accountNumber, decimal amount)
        {
            var account = await unitOfWork.Wallets.GetAsync(wallet => wallet.Id == accountNumber);
            if (account == null) return false;

            var newBalance = account.Balance + amount;
            if (account.IsIdentified && newBalance > 100000) return false;
            if (!account.IsIdentified && newBalance > 10000) return false;

            account.Balance = newBalance;
            unitOfWork.Wallets.Update(account);

            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Transaction>> GetRechargeSummaryAsync(Guid walletId)
        {
            var currentMonth = DateTime.Now.Month;

            var result = unitOfWork.Transaction.GetAll(act => act.ToWalletId == walletId && act.Date.Month == currentMonth).AsEnumerable();

            return result;
        }

        public async Task<decimal> GetAccountBalanceAsync(Guid accountNumber)
        {
            var account = await unitOfWork.Wallets.GetAsync(wallet => wallet.Id == accountNumber);

            if (account == null) return 0;

            return account.Balance;
        }
    }
}

