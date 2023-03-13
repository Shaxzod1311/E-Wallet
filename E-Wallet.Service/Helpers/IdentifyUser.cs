using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Service.Helpers
{
    public class IdentifyUser
    {
        private HttpContextHelper HttpContext;
        private ClaimsPrincipal Claims;

        public Guid Id { get; set; }
        public string? Username { get; set; }
        public bool IsIdentified { get; set; }


        public IdentifyUser(HttpContextHelper httpContext)
        {
            this.HttpContext = httpContext;

            Claims = HttpContext.Context.User;
            Id = Guid.Parse(Claims.FindFirst("Id").Value);
            Username = Claims.FindFirst("Username").Value.ToString();
            IsIdentified = bool.Parse(Claims.FindFirst("IsIdentified").Value);
        }
    }
}
