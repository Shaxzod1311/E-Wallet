namespace E_Wallet.Domain.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public decimal MaxAmount { get; set; } = 10.000M;
        public DateTime CreateDate { get; set; }
    }
}