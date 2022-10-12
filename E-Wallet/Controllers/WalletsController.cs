using E_Wallet.Data.Repositories;
using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]/action")]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService walletService;

        public WalletsController(IWalletService walletService)
        {
            this.walletService = walletService;
        }


        [HttpPost]
        public async Task<ActionResult<BaseResponse<decimal>>> BalanceOfWalletAsync(Guid walletId)
        {
            return Ok(await walletService.BalanceOfWalletAsync(walletId));
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<Guid>>> CheckWalletExistingAsync(string username, string password)
        {
            return Ok(await walletService.CheckWalletExistingAsync(username, password));
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<bool>>> FillWalletAsync(decimal amount, Guid walletId)
        {
            return Ok(await walletService.FillWalletAsync(amount, walletId)); 
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<IEnumerable<IncomeDTO>>>> FullMonthIncomeAsync(Guid walletId, DateTime date)
        {
            return Ok(await walletService.FullMonthIncomeAsync(walletId, date));
        }
    }
}
