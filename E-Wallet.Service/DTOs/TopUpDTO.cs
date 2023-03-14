

namespace E_Wallet.Service.DTOs
{
    public class TopUpDTO
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
    }
}
