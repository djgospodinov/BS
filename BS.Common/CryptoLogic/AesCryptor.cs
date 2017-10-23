﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BS.Common
{
    /// <summary>
    /// https://nciphers.com/aes/
    /// </summary>
    public class AesCryptor : ICryptor
    {
        private readonly int _saltSize = 32;
 
        public string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText");
            }
 
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
 
            using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, _saltSize))
            {
                byte[] saltBytes = keyDerivationFunction.Salt;
                byte[] keyBytes = keyDerivationFunction.GetBytes(32);
                byte[] ivBytes = keyDerivationFunction.GetBytes(16);
 
                using (var aesManaged = new AesManaged())
                {
                    aesManaged.KeySize = 256;
                    aesManaged.Mode = CipherMode.CBC;
                    aesManaged.Padding = PaddingMode.PKCS7;
 
                    using (var encryptor = aesManaged.CreateEncryptor(keyBytes, ivBytes))
                    {
                        MemoryStream memoryStream = null;
                        CryptoStream cryptoStream = null;
 
                        return WriteMemoryStream(plainText, ref saltBytes, encryptor, ref memoryStream, ref cryptoStream);
                    }
                }
            }
        }
 
        public string Decrypt(string ciphertext, string key)
        {
            if (string.IsNullOrEmpty(ciphertext))
            {
                throw new ArgumentNullException("ciphertext");
            }
 
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
 
            var allTheBytes = Convert.FromBase64String(ciphertext);
            var saltBytes = allTheBytes.Take(_saltSize).ToArray();
            var ciphertextBytes = allTheBytes.Skip(_saltSize).Take(allTheBytes.Length - _saltSize).ToArray();
 
            using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, saltBytes))
            {
                var keyBytes = keyDerivationFunction.GetBytes(32);
                var ivBytes = keyDerivationFunction.GetBytes(16);
 
                return DecryptWithAES(ciphertextBytes, keyBytes, ivBytes);
            }
        }
 
        private string WriteMemoryStream(string plainText, ref byte[] saltBytes, ICryptoTransform encryptor, ref MemoryStream memoryStream, ref CryptoStream cryptoStream)
        {
            try
            {
                memoryStream = new MemoryStream();
 
                try
                {
                    cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
 
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                }
                finally
                {
                    if (cryptoStream != null)
                    {
                        cryptoStream.Dispose();
                    }
                }
 
                var cipherTextBytes = memoryStream.ToArray();
                Array.Resize(ref saltBytes, saltBytes.Length + cipherTextBytes.Length);
                Array.Copy(cipherTextBytes, 0, saltBytes, _saltSize, cipherTextBytes.Length);
 
                return Convert.ToBase64String(saltBytes);
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }
        }
 
        private static string DecryptWithAES(byte[] ciphertextBytes, byte[] keyBytes, byte[] ivBytes)
        {
            using (var aesManaged = new AesManaged())
            {
                using (var decryptor = aesManaged.CreateDecryptor(keyBytes, ivBytes))
                {
                    MemoryStream memoryStream = null;
                    CryptoStream cryptoStream = null;
                    StreamReader streamReader = null;
 
                    try
                    {
                        memoryStream = new MemoryStream(ciphertextBytes);
                        cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                        streamReader = new StreamReader(cryptoStream);
 
                        return streamReader.ReadToEnd();
                    }
                    finally
                    {
                        if (memoryStream != null)
                        {
                            memoryStream.Dispose();
                            memoryStream = null;
                        }
                    }
                }
            }
        }
    }
}