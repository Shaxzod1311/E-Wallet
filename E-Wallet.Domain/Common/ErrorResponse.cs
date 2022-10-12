namespace E_Wallet.Domain.Common
{
    public class ErrorResponse
    {
        public ErrorResponse(int? code = null, string? mesage = null)
        {
            Code = code;
            Message = mesage;
        }

        public int? Code { get; set; }
        public string? Message { get; set; }

    }
}