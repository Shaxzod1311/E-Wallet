using E_Wallet.Domain.Common;
using E_Wallet.Domain.Models;
using E_Wallet.Service.DTOs;

namespace E_Wallet.Service.Interfaces
{
    public interface IWalletService
    {
        Task<WalletDTO> CheckAccountExistsAsync(Guid accountNumber);
        Task<bool> ToUpAccountAsync(Guid accountNumber, decimal amount);
        Task<IEnumerable<Transaction>> GetRechargeSummaryAsync(Guid walletId);
        Task<decimal> GetAccountBalanceAsync(Guid accountNumber);
    }
}
