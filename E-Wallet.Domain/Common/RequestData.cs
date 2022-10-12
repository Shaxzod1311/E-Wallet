using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Domain.Common
{
    public class RequestData
    {
        public string? HttpMethod { get; set; }
        public string? ClientId { get; set; }
        public string? Nonce { get; set; }
        public string? RequestUrl { get; set; }
        public DateTime? RequestTimestamp { get; set; }
        public string? RequestBodyHash { get; set; }
    }
}
