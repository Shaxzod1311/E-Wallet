using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Interfaces;
using E_Wallet.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Wallet.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<WalletDTO>>> GetBalance(Guid accounNumber)
        {
            return await walletService.GetAccountBalanceAsync(accounNumber);

        }

        [HttpPost("{id}/topUp")]
        public async Task<ActionResult<BaseResponse<Guid>>> TopUpAsync(Guid walletId, [FromBody] decimal amount)
        {
            return await transactionService.TopUpWalletAsync(walletId, amount);
        }

        [HttpGet("{id}/rechargeinfo")]
        public async Task<ActionResult<BaseResponse<IEnumerable<TransactionDTO>>>> GetRechargeInfo(Guid walletId)
        {
            return await transactionService.GetAllTransactionForcCurrentMonth(walletId);
        }


        [HttpGet()]
        public async Task<ActionResult<BaseResponse<WalletDTO>>> CheckToAccountExists()
        {
            return await userService.CheckAccountExistsAsync();
        }
    }
}
