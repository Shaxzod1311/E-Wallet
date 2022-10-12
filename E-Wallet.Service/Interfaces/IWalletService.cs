using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;

namespace E_Wallet.Service.Interfaces
{
    public interface IWalletService
    {
        Task<BaseResponse<Guid>> CheckWalletExistingAsync(string username, string password);
        Task<BaseResponse<bool>> FillWalletAsync(decimal amount, Guid walletId);
        Task<BaseResponse<IEnumerable<IncomeDTO>>> FullMonthIncomeAsync(Guid walletId, DateTime month);
        Task<BaseResponse<decimal>> BalanceOfWalletAsync(Guid walletId);
    }
}
