using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OpenSshKey.Parser
{
    static class AesDecrypt
    {
        public static byte[] CbcModeDecrypt(byte[] encryptedBytes, byte[] key, byte[] iv)
        {
            //adapted from https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aesmanaged
            if (encryptedBytes == null || encryptedBytes.Length <= 0)
                throw new ArgumentNullException("encryptedBytes");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            byte[] decryptedBytes = null;

            using (var aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Padding = PaddingMode.None;
                aes.Mode = CipherMode.CBC;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var brDecrypt = new BinaryReader(csDecrypt, Encoding.UTF8))
                        {
                            decryptedBytes = brDecrypt.ReadBytes((int)msDecrypt.Length);
                        }
                    }
                }
            }
            return decryptedBytes;
        }

        public static byte[] CtrModeDecrypt(byte[] encryptedBytes, byte[] key, byte[] iv)
        {
            if (encryptedBytes == null || encryptedBytes.Length <= 0)
                throw new ArgumentNullException("encryptedBytes");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            var decryptedBytes = new byte[encryptedBytes.Length];
            using (var aesCounterMode = new AesCounterMode(iv))
            {
                using (var cryptoTransform = aesCounterMode.CreateDecryptor(key, null))
                {
                    cryptoTransform.TransformBlock(encryptedBytes, 0, encryptedBytes.Length, decryptedBytes, 0);
                }
            }
            return decryptedBytes;
        }
    }
}
