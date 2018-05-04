using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BS.Api
{
    /// <summary>
    /// https://www.base64decode.org/
    /// </summary>
    public class Base64Helper
    {
        public static string ToBase64String(string encodedString)
        {
            var result = Convert.ToBase64String(Encoding.UTF8.GetBytes(Reverse(encodedString)));
            return result;
        }

        public static string FromBase64String(string result)
        {
            byte[] data = Convert.FromBase64String(Reverse(result));
            string decodedString = Encoding.UTF8.GetString(data);

            return decodedString;
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}