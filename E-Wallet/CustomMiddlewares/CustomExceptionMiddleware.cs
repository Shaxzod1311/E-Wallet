
using E_Wallet.Domain.Common;
using Newtonsoft.Json;

namespace E_Wallet.CustomMiddleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                await HandleExceptionAsync(context, ex);

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);

            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCodeException exception)
        {
            context.Response.ContentType = "application/json";
            ErrorResponse result = new ErrorResponse()
            {
                Message = exception.Message,
                Code = exception.StatusCode
            };

            context.Response.StatusCode = exception.StatusCode;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            ErrorResponse result = new ErrorResponse()
            {
                Message = "server error",
                Code = 500
            };

            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
