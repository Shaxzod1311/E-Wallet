using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Interfaces;
using E_Wallet.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Wallet.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EwalletController : ControllerBase
    {
        private readonly ITrasnactionService transactionService;
        private readonly IWalletService walletService;
        private readonly IUserService userService;


        public EwalletController(ITrasnactionService trasnactionService, IWalletService walletService, IUserService userService)
        {
            this.transactionService = trasnactionService;
            this.walletService = walletService;
            this.userService = userService;
        }

        [HttpPost()]
        public async Task<ActionResult<BaseResponse<WalletDTO>>> GetBalance([FromBody] Guid accounNumber)
        {
            return await walletService.GetAccountBalanceAsync(accounNumber);

        }

        [HttpPost()]
        public async Task<ActionResult<BaseResponse<Guid>>> TopUpAsync(TopUpDTO topUpDTO)
        {
            return await transactionService.TopUpWalletAsync(topUpDTO);
        }

        [HttpPost()]
        public async Task<ActionResult<BaseResponse<IEnumerable<TransactionDTO>>>> GetRechargeInfo([FromBody] Guid walletId)
        {
            return await transactionService.GetAllTransactionForcCurrentMonth(walletId);
        }


        [HttpPost()]
        public async Task<ActionResult<BaseResponse<WalletDTO>>> CheckToAccountExists()
        {
            return await userService.CheckAccountExistsAsync();
        }
    }
}
