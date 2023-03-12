using E_Wallet.Data.IRepositories;
using E_Wallet.Domain.Common;
using E_Wallet.HmacService;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace E_Wallet.CustomMiddleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);

            if (authHeader.Scheme != "HMAC")
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var authToken = authHeader.Parameter;
            var authInfo = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
            var userId = Guid.Parse(authInfo.Split(':')[0]);
            var digest = authInfo.Split(':')[1];

            var user = await userRepository.GetAsync(user => user.Id == userId);

            if (user == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(user.SecretKey));
            var requestContent = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestContent));
            var hashString = Convert.ToBase64String(hashBytes);

            if (hashString != digest)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            context.Items["User"] = user;

            await _next(context);
        }
    }

}
