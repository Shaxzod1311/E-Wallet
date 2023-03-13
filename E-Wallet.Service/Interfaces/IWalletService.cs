

using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;

namespace E_Wallet.Service.Interfaces
{
    public interface IWalletService
    {
        Task<BaseResponse<WalletDTO>> GetAccountBalanceAsync(Guid accountNumber);
    }
}
