using Microsoft.AspNetCore.Http;

namespace E_Wallet.Service.Helpers
{
    public class HttpContextHelper
    {
        public IHttpContextAccessor Accessor { get; }
        public HttpContext Context { get; }
        public IHeaderDictionary ReponseHeaders { get; }
        public string HostUrl { get; }

        public HttpContextHelper(IHttpContextAccessor accessor)
        {
            Accessor = accessor;
            Context = Accessor.HttpContext;
        }
    }
}
