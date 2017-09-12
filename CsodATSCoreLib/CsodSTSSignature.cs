using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace CsodATSCoreLib
{
    class CsodSTSSignature
    {
        public string SignRequest(string secret, string method, NameValueCollection headers, string url)
        {
            //url = string.Format("/devrelease{0}", url);
            var stringToSign = ConstructStringToSign(method.ToString(), headers, url);
            var sig = SignString512(stringToSign, secret);
            return sig;
        }

        private string ConstructStringToSign(string httpMethod, NameValueCollection headers, string pathAndQuery)
        {
            StringBuilder stringToSign = new StringBuilder();
            var httpVerb = httpMethod.Trim() + "\n";
            var csodHeaders = headers.Cast<string>().Where(w => w.StartsWith("x-csod-"))
                                                    .Where(w => w != "x-csod-signature")
                                                    .Distinct()
                                                    .OrderBy(s => s)
                                                    .Aggregate(string.Empty, (a, l) => a + l.ToLower().Trim() + ":" + headers[l].Trim() + "\n");
            stringToSign.Append(httpVerb);
            stringToSign.Append(csodHeaders);
            stringToSign.Append(pathAndQuery);
            return stringToSign.ToString();
        }
        private string SignString512(string stringToSign, string secretKey)
        {
            byte[] secretkeyBytes = Convert.FromBase64String(secretKey);
            byte[] inputBytes = Encoding.UTF8.GetBytes(stringToSign);
            using (var hmac = new HMACSHA512(secretkeyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                return System.Convert.ToBase64String(hashValue);
            }
        }
    }
}
