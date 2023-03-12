using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Service.DTOs
{
    public class GetRechargeSummaryResponseDTO
    {
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
