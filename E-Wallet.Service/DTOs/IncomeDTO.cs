﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Service.DTOs
{
    public class IncomeDTO
    {
        public Guid UserId { get; set; }
        public Guid ToWalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid FromWalletId { get; set; }
    }
}
