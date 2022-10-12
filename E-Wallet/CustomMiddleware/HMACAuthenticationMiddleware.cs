using E_Wallet.Domain.Common;
using E_Wallet.HmacService;


namespace E_Wallet.CustomMiddleware
{
    public class HMACAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration config;


        public HMACAuthenticationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            this.config = config;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var headerKeys = context.Request.Headers;

            string clientId = headerKeys.Select(key => key.Value).Where(key => key == "X-UserId").ToString();
            string requestHash = headerKeys.Select(key => key.Value).Where(key => key == "X-Digest").ToString();

            string ClientSecret = config.GetSection("Authorization").Get<Dictionary<string, string>>().Where(item => item.Value == clientId).Select(item => item.Value).FirstOrDefault();

            bool checkAuthentication = HMACAuthenticationHelper.IsValidRequest(context.Request, ClientSecret, requestHash, "", "");

            if (!checkAuthentication)
                throw new HttpStatusCodeException(401, "UnAuthorized");

            await _next(context);
        }
    }
}
