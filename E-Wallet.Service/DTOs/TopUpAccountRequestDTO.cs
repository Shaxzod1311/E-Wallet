using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Service.DTOs
{
    public class TopUpAccountRequestDTO
    {
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Amount { get; set; }
    }
}
