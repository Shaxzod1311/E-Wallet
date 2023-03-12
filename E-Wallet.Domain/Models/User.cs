using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Wallet.Domain.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        
        [ForeignKey(nameof(Wallet))]
        public Guid WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        public string SecretKey { get; set; }
    }
}
