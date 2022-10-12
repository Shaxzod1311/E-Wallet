namespace E_Wallet.Domain.Common
{
    public class BaseResponse<TSource>
    {
        public int? Code { get; set; } = 200;
        public TSource? Data { get; set; }
        public ErrorResponse? Error { get; set; }

    }
}
