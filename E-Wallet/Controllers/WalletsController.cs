using E_Wallet.Data.Repositories;
using E_Wallet.Domain.Common;
using E_Wallet.Service.DTOs;
using E_Wallet.Service.Interfaces;
using E_Wallet.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EWalletController : ControllerBase
    {
        private readonly WalletService _service;

        public EWalletController(WalletService service)
        {
            _service = service;
        }

        // 1. Check if the e-wallet account exists.
        [HttpPost("check")]
        public async Task<IActionResult> CheckAccountExistsAsync([FromBody] CheckAccountExistsRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var exists = await _service.CheckAccountExistsAsync(request.AccountNumber);
            return Ok(new CheckAccountExistsResponse { Exists = exists });
        }

        // 2. Replenish e-wallet account.
        [HttpPost("replenish")]
        public async Task<IActionResult> ReplenishAccountAsync([FromBody] ReplenishAccountRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var success = await _service.ReplenishAccountAsync(request.AccountNumber, request.Amount);
            if (!success) return BadRequest();

            return Ok();
        }

        // 3. Get the total number and amount of recharge operations for the current month.
        [HttpPost("recharge-summary")]
        public async Task<IActionResult> GetRechargeSummaryAsync(Guid walletId)
        {
            var summary = await _service.GetRechargeSummaryAsync(walletId);
            return Ok(new GetRechargeSummaryResponse {  });
        }

        // 4. Get the e-wallet balance.
        [HttpPost("balance")]
        public async Task<IActionResult> GetAccountBalanceAsync([FromBody] GetAccountBalanceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var balance = await _service.GetAccountBalanceAsync(request.AccountNumber);
            return Ok(new GetAccountBalanceResponse { Balance = balance });
        }
    }
}

