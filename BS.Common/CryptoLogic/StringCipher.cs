using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common
{
    public interface ICryptor
    {
        string Decrypt(string ciphertext, string key);
        string Encrypt(string plainText, string key);
    }

    public static class StringCipher
    {

        private static ICryptor _cryptor = new AesCryptor();

        public static string Encrypt(string plainText, string passPhrase)
        {
            return _cryptor.Encrypt(plainText, passPhrase);
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            return _cryptor.Decrypt(cipherText, passPhrase);
        }
    }
}
