using E_Wallet.Service.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using System.Runtime.Caching;

namespace E_Wallet.HmacService
{
    public static class HMACAuthenticationHelper
    {

        private static readonly UInt64 requestMaxAgeInSeconds = 300; //Means 5 min

        public static bool IsValidRequest(HttpRequest req, string keySecret, string incomingBase64Signature, string nonce, string requestTimeStamp)
        {
            string requestContentBase64String = "";
            string requestUri = req.GetEncodedUrl();
            string requestHttpMethod = req.Method;


            if (IsReplayRequest(nonce, requestTimeStamp))
            {
                return false;
            }

            byte[] hash = Convert.FromBase64String(req.Body.ToString().ToHmacSha1(keySecret));

            if (hash != null)
            {
                requestContentBase64String = Convert.ToBase64String(hash);
            }

            string data = String.Format("{0}{1}{2}{3}{4}{5}", keySecret, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            string signature = data.ToHmacSha1(keySecret);
            
            return incomingBase64Signature.Equals(signature, StringComparison.Ordinal);
        }


        public static bool IsReplayRequest(string nonce, string requestTimeStamp)
        {
            if (MemoryCache.Default.Contains(nonce))
            {
                return true;
            }

            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);

            TimeSpan currentTs = DateTime.UtcNow - epochStart;

            var serverTotalSeconds = Convert.ToUInt64(currentTs.TotalSeconds);

            var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

            if ((serverTotalSeconds - requestTotalSeconds) > requestMaxAgeInSeconds)
            {
                return true;

            }

            MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(requestMaxAgeInSeconds));

            return false;
        }

    }
}

