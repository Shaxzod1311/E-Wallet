using AutoMapper;
using E_Wallet.Data.IRepositories;
using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Xml.XPath;

namespace E_Wallet.Service.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork? unitOfWork;

        public WalletService(IUnitOfWork? unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<BaseResponse<decimal>> BalanceOfWalletAsync(Guid walletId)
        {
            BaseResponse<decimal> response = new BaseResponse<decimal>();

            var result = await unitOfWork.Wallets.GetAsync(wallet => wallet.Id == walletId);

            if (result == null)
                throw new HttpStatusCodeException(404, "Wallet not found");

            response.Data = result.Amount;

            return response;
        }

        public async Task<BaseResponse<Guid>> CheckWalletExistingAsync(string username, string password)
        {
            BaseResponse<Guid> response = new BaseResponse<Guid>();

            var result = await unitOfWork.Users.GetAll(user => user.Password == password && user.Username == username).Include(user => user.Wallet).Select(wallet => wallet.WalletId).FirstOrDefaultAsync();

            if (result == null)
                throw new HttpStatusCodeException(404, "Wallet not found");

            response.Data = result;

            return response;
        }

        public async Task<BaseResponse<bool>> FillWalletAsync(decimal amount, Guid walletId)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();

            var result = await unitOfWork.Wallets.GetAsync(wallet => wallet.Id == walletId);

            if (result == null)
                throw new HttpStatusCodeException(404, "wallet not found");

            if (amount + result.Amount > result.MaxAmount)
                throw new HttpStatusCodeException(79, "transmission failure");
            
            result.Amount += amount;

            response.Data = true;

            return response;
        }

        public async Task<BaseResponse<IEnumerable<IncomeDTO>>> FullMonthIncomeAsync(Guid walletId, DateTime date)
        {
            BaseResponse<IEnumerable<IncomeDTO>> response = new BaseResponse<IEnumerable<IncomeDTO>>();


            var result = unitOfWork.Incomes.GetAll(income => income.ToWalletId == walletId && income.Date.Month == date.Month).ToList();

            if (result == null)
                throw new HttpStatusCodeException(404, "Wallet not found");
            
            response.Data = result;
        }
    }
}
