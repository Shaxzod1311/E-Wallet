using System.Security.Cryptography;
using System.Text;


namespace E_Wallet.Service.Extensions
{
    public static class StringExtension
    {
        public static string ToHmacSha1(this string input, string key)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            byte[] keyEncoded  = Convert.FromBase64String(key);

            using (var myhmacsha1 = new HMACSHA1(keyEncoded))
            {
                var hashArray = myhmacsha1.ComputeHash(byteArray);

                return hashArray.Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            }
        }
    }
}
