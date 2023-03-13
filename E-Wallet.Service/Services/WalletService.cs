using AutoMapper;
using E_Wallet.Data.IRepositories;
using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Interfaces;



namespace E_Wallet.Service.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork? unitOfWork;
        private readonly IMapper mapper;

        public WalletService(IUnitOfWork? unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<BaseResponse<WalletDTO>> GetAccountBalanceAsync(Guid accountNumber)
        {
            BaseResponse<WalletDTO> response = new BaseResponse<WalletDTO>();

            var account = await unitOfWork.Wallets.GetAsync(wallet => wallet.Id == accountNumber);

            if (account == null) return response;

            response.Data = mapper.Map<WalletDTO>(account);

            return response;
        }
    }
}

