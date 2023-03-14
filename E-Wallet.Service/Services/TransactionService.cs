

using AutoMapper;
using E_Wallet.Data.IRepositories;
using E_Wallet.Data.Repositories;
using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Helpers;
using E_Wallet.Service.Interfaces;
using System.Security.Principal;

namespace E_Wallet.Service.Services
{
    public class TransactionService : ITrasnactionService
    {

        private readonly IUnitOfWork? unitOfWork;
        private readonly IMapper mapper;
        private readonly IdentifyUser User;

        public TransactionService(IUnitOfWork? unitOfWork, IMapper mapper, IdentifyUser User)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.User = User;
        }


        public async Task<BaseResponse<IEnumerable<TransactionDTO>>> GetAllTransactionForcCurrentMonth(Guid walletId)
        {
            BaseResponse<IEnumerable<TransactionDTO>> response = new BaseResponse<IEnumerable<TransactionDTO>>();
            var currentMonth = DateTime.Now.Month;

            var transactions = unitOfWork.Transaction.GetAll(trans => trans.WalletId == walletId &&
                                                             trans.Date.Month == currentMonth &&
                                                             trans.Type == Domain.Enums.TransactionTypes.TopUp).ToList();
            if (transactions.Count() == 0) 
            {
                throw new HttpStatusCodeException(404, "not_found_error");
            }

            response.Data = mapper.Map<IEnumerable<TransactionDTO>>(transactions);
            return response;

        }

        public async Task<BaseResponse<Guid>> TopUpWalletAsync(TopUpDTO topUpDTO)
        {
            BaseResponse<Guid> response = new BaseResponse<Guid>();

            var wallet = unitOfWork.Wallets.GetAll(wallet => wallet.Id == topUpDTO.WalletId).FirstOrDefault();

            if (wallet == null)
                throw new HttpStatusCodeException(404, "not_found_error");

            var newBalance = wallet.Balance + topUpDTO.Amount;

            if (User.IsIdentified && newBalance > 100000)
            {
                throw new HttpStatusCodeException(409, "value_out_of_range_max_is_100000");
            }
            else if (!User.IsIdentified && newBalance > 10000)
            {
                throw new HttpStatusCodeException(409, "value_out_of_rangemax_is_10000");
            }

            wallet.Balance = newBalance;

            var updatedWallet = unitOfWork.Wallets.Update(wallet);

            await unitOfWork.SaveChangesAsync();

            response.Data = updatedWallet.Id;

            return response;
        }
    }
}
