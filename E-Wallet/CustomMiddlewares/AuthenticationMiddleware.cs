using E_Wallet.Data.IRepositories;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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

            var userId = context.Request.Headers["X-UserId"];
            var digest = context.Request.Headers["X-Digest"];

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(digest))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var user = await userRepository.GetAsync(user => user.Id == Guid.Parse(userId));

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

            var claims = new List<Claim>
            {

                new Claim("Id", userId.ToString()),
                new Claim("Username",  userId.ToString()),
                new Claim("IsIdentified", "true"),

            };

            var identity = new ClaimsIdentity(claims, "User");
            var claimPrincipal = new ClaimsPrincipal(identity);

            context.User = claimPrincipal;

            byte[] byteArray = Encoding.UTF8.GetBytes(requestContent);
            MemoryStream stream = new MemoryStream(byteArray);

            context.Request.Body = stream;

            await _next(context);
        }
    }

}
