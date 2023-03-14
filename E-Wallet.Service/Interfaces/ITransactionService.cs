
using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;

namespace E_Wallet.Service.Interfaces
{
    public interface ITrasnactionService
    {
        Task<BaseResponse<Guid>> TopUpWalletAsync(TopUpDTO topUpDTO);
        Task<BaseResponse<IEnumerable<TransactionDTO>>> GetAllTransactionForcCurrentMonth(Guid walletId);
    }
}