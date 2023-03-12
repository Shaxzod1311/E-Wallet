using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace E_Wallet.Domain.Models
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(ToUser))]
        public Guid ToUserId { get; set; }
        
        [ForeignKey(nameof(ToUser))]
        public Guid FromUserId { get; set; }
        public Guid ToWalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid FromWalletId { get; set; }
        public User ToUser { get; set; }
        public User FromUser { get; set; }
    }
}
