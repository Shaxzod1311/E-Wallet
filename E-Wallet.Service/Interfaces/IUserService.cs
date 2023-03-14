using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;

namespace E_Wallet.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<WalletDTO>> CheckAccountExistsAsync(Guid userId);
    }
}
