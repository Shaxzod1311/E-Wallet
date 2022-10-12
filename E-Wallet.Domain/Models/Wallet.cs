using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Wallet.Domain.Models
{
    public class Wallet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public decimal MaxAmount { get; set; } = 10.000M;
        public DateTime CreateDate { get; set; }
        public bool Isbocked { get; set; }
    }
}