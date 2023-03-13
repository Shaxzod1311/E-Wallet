

using AutoMapper;
using E_Wallet.Data.IRepositories;
using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Helpers;
using E_Wallet.Service.Interfaces;

namespace E_Wallet.Service.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork? unitOfWork;
        private readonly IMapper mapper;
        private readonly IdentifyUser User;

        public UserService(IUnitOfWork? unitOfWork, IMapper mapper, IdentifyUser User)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.User = User;
        }

        public async Task<BaseResponse<WalletDTO>> CheckAccountExistsAsync()
        {
            BaseResponse<WalletDTO> response = new BaseResponse<WalletDTO>();

            var wallet = await unitOfWork.Wallets.GetAsync(wallet => wallet.UserId == User.Id);

            if (wallet == null) 
            {
                throw new HttpStatusCodeException(404, "not_found_error");
            }

            response.Data = mapper.Map<WalletDTO>(wallet);
            return response;
        }

    }
}
