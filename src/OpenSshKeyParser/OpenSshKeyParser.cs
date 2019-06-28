using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenSshKey.Parser
{
    public static class OpenSshKeyParser
    {
        private const string BEGIN_HEADER = "-----BEGIN OPENSSH PRIVATE KEY-----";
        private const string END_HEADER = "-----END OPENSSH PRIVATE KEY-----";
        private const string OPEN_SSH_KEY_V1 = "openssh-key-v1";
        private static byte[] AUTH_MAGIC = Encoding.UTF8.GetBytes($"{OPEN_SSH_KEY_V1}\0");
        private static string AES256_CBC = "aes256-cbc";
        private static string AES256_CTR = "aes256-ctr";

        public static IKeyPair ParseOpenSshKeyFile(string privateKeyFileText, string passPhrase = null)
        {
            ValidPrivateKeyFileText(privateKeyFileText);
            try
            {
                IKeyPair keyPair = null;
                ValidateHeader(privateKeyFileText);
                using (var keyFileReader = GetPrivateKeyFileDataReader(privateKeyFileText))
                {
                    ValidateMagicHeader(keyFileReader.ReadBytes(AUTH_MAGIC.Length));

                    var cipherName = keyFileReader.ReadString(Encoding.UTF8);
                    var kdfName = keyFileReader.ReadString(Encoding.UTF8);
                    byte[] salt = null;
                    var rounds = 0;
                    if (keyFileReader.ReadInt32() > 0) //kdf options
                    {
                        salt = keyFileReader.ReadBytes(keyFileReader.ReadInt32());
                        rounds = keyFileReader.ReadInt32();
                    }
                    ValidateEncryptionFields(cipherName, kdfName, passPhrase);

                    ValidateNumberOfKeys(keyFileReader.ReadInt32());

                    keyFileReader.ReadBytes(keyFileReader.ReadInt32()); //skip public key section

                    //we skipped the public key section but for reference, here is what parsing it would look like:
                    /*
                    using (var publicKeySectionReader = new PrivateKeyFileDataReader(keyFileReader.ReadBytes(keyFileReader.ReadInt32())))
                    {
                        publicKeySectionReader.ReadString(Encoding.UTF8); //key type
                        publicKeySectionReader.ReadBytes(); //public key, the parts of which will be dependant on the key type
                    }
                    */

                    var privateKeySectionBytes = keyFileReader.ReadBytes(keyFileReader.ReadInt32());
                    if (cipherName != "none")
                    {
                        privateKeySectionBytes = DecryptPrivateKeySection(cipherName, privateKeySectionBytes, salt, rounds, passPhrase);
                    }
                    ValidatePrivateKeySectionLength(privateKeySectionBytes.Length);

                    using (var privateKeySectionReader = new PrivateKeyFileDataReader(privateKeySectionBytes))
                    {
                        ValidateCheckInts(privateKeySectionReader.ReadInt32(), privateKeySectionReader.ReadInt32());
                        keyPair = ExtractKeyPair(privateKeySectionReader);
                        ValidatePrivateKeyPadding(privateKeySectionReader.ReadBytes());
                    }
                }
                return keyPair;
            }
            catch(Exception e)
            {
                if(e is OpenSshKeyParseException)
                {
                    throw e;
                }
                throw new OpenSshKeyParseException($"unexpected failure parsing key: {e.Message}");
            }
        }

        private static string GetBase64Content(string privateKeyFileText)
        {
            var base64Content = new StringBuilder();
            using (var stringReader = new StringReader(privateKeyFileText))
            {
                var headerLine = stringReader.ReadLine();
                while (headerLine != null && headerLine != BEGIN_HEADER)
                {
                    headerLine = stringReader.ReadLine();
                }

                if (headerLine != null)
                {
                    var base64Line = stringReader.ReadLine();
                    while (base64Line != null && base64Line != END_HEADER)
                    {
                        base64Content.AppendLine(base64Line);
                        base64Line = stringReader.ReadLine();
                    }
                }
            }
            return base64Content.ToString();
        }

        private static PrivateKeyFileDataReader GetPrivateKeyFileDataReader(string privateKeyFileText) =>
            new PrivateKeyFileDataReader(Convert.FromBase64String(GetBase64Content(privateKeyFileText)));

        private static void ValidateHeader(string privateKeyFileText)
        {
            var valid = false;
            using(var stringReader = new StringReader(privateKeyFileText))
            {
                var line = stringReader.ReadLine();
                while(line != null && line != BEGIN_HEADER)
                {
                    line = stringReader.ReadLine();
                }
                if(line == BEGIN_HEADER)
                {
                    valid = true;
                }
            }
            if(!valid)
            {
                throw new OpenSshKeyParseException("expected header was not found in key");
            }
        }

        private static void ValidPrivateKeyFileText(string privateKeyFileText)
        {
            if (string.IsNullOrEmpty(privateKeyFileText))
            {
                throw new ArgumentException("text of private key file cannot be null or empty", "privateKeyFileText");
            }
        }

        private static void ValidateMagicHeader(byte[] magicHeader)
        {            
            if(!AUTH_MAGIC.SequenceEqual(magicHeader))
            {
                throw new OpenSshKeyParseException($"key does not contain the '{OPEN_SSH_KEY_V1}' format magic header");
            }
        }

        private static void ValidateEncryptionFields(string cipherName, string kdfName, string passPhrase)
        {
            if(cipherName != "none")
            {
                if (string.IsNullOrEmpty(passPhrase))
                {
                    throw new OpenSshKeyParseException("private key section is encrypted but passphrase is empty");
                }
                if (string.IsNullOrEmpty(kdfName) || kdfName != "bcrypt")
                {
                    throw new OpenSshKeyParseException("kdf " + kdfName + " is not supported");
                }
                if (cipherName != AES256_CBC && cipherName != AES256_CTR)
                {
                    throw new OpenSshKeyParseException("cipher name " + cipherName + " is not supported by this parser");
                }
            }
        }

        private static void ValidatePrivateKeyPadding(byte[] padding)
        {
            //The list of privatekey/comment pairs is padded with the bytes 1, 2, 3, ... until the total length is a multiple of the cipher block size.
            for (int i = 0; i < padding.Length; i++)
            {
                if ((int)padding[i] != i + 1)
                {
                    throw new OpenSshKeyParseException($"padding of private keys section contained wrong byte at position: {i}");
                }
            }
        }

        private static void ValidateNumberOfKeys(int numberOfKeys)
        {
            if (numberOfKeys != 1)
            {
                throw new OpenSshKeyParseException("only one public/private key pair is supported by this parser");
            }
        }

        private static void ValidatePrivateKeySectionLength(int privateKeyLength)
        {
            if (privateKeyLength % 8 != 0)
            {
                throw new OpenSshKeyParseException("the private key section must be a multiple of the block size (8)");
            }
        }

        private static void ValidateCheckInts(int checkInt1, int checkInt2)
        {
            if (checkInt1 != checkInt2)
            {
                throw new OpenSshKeyParseException("checkints differed, the private key section was not correctly decoded (check your passphrase)");
            }
        }

        private static byte[] DecryptPrivateKeySection(string cipher,
            byte[] encryptedPrivateKeySection, byte [] salt, int rounds, string passPhrase)
        {
            //extracting key/iv from kdf was adapted from and inspired by the SSHj library (https://github.com/hierynomus/sshj)
            var passPhraseBytes = Encoding.UTF8.GetBytes(passPhrase);
            byte[] keyiv = new byte[48];
            new BCrypt().Pbkdf(passPhraseBytes, salt, rounds, keyiv);
            byte[] key = new byte[32];
            Array.Copy(keyiv, 0, key, 0, 32);
            byte[] iv = new byte[16];
            Array.Copy(keyiv, 32, iv, 0, 16);

            byte[] decryptedBytes = null;
            if(cipher == AES256_CBC)
            {
                decryptedBytes = AesDecrypt.CbcModeDecrypt(encryptedPrivateKeySection, key, iv);
            }
            else if(cipher == AES256_CTR)
            {
                decryptedBytes = AesDecrypt.CtrModeDecrypt(encryptedPrivateKeySection, key, iv);
            }
            return decryptedBytes;
        }

        private static IKeyPair ExtractKeyPair(PrivateKeyFileDataReader privateKeySectionReader)
        {
            var keyType = privateKeySectionReader.ReadString(Encoding.UTF8);
            IKeyPair keyPair = null;
            if (keyType == KeyTypes.ED25519)
            {
                var publicKey = privateKeySectionReader.ReadBytes(privateKeySectionReader.ReadInt32());
                privateKeySectionReader.ReadInt32(); //length of private + public key
                var privateKey = privateKeySectionReader.ReadBytes(32);
                privateKeySectionReader.ReadBytes(32); //public key (again)
                var comment = privateKeySectionReader.ReadString(Encoding.UTF8);
                keyPair = new Ed25519KeyPair(comment, publicKey, privateKey);
            }
            else if (keyType == KeyTypes.RSA)
            {
                var n = privateKeySectionReader.ReadBigInteger(); //Modulus
                var e = privateKeySectionReader.ReadBigInteger(); //Public Exponent
                var d = privateKeySectionReader.ReadBigInteger(); //Private Exponent
                var iqmp = privateKeySectionReader.ReadBigInteger(); //q^-1 mod p
                var p = privateKeySectionReader.ReadBigInteger(); //Prime 1
                var q = privateKeySectionReader.ReadBigInteger(); //Prime 2
                var comment = privateKeySectionReader.ReadString(Encoding.UTF8);
                keyPair = new RsaKeyPair(comment, n, e, d, iqmp, p, q);
            }
            else if (keyType == KeyTypes.ECDSA256 || keyType == KeyTypes.ECDSA384 || keyType == KeyTypes.ECDSA521)
            {
                int curveLength = privateKeySectionReader.ReadInt32();
                var curve = Encoding.ASCII.GetString(privateKeySectionReader.ReadBytes(curveLength));
                var publicKey = privateKeySectionReader.ReadBytes(privateKeySectionReader.ReadInt32());
                var privateKey = privateKeySectionReader.ReadBytes(privateKeySectionReader.ReadInt32());
                var comment = privateKeySectionReader.ReadString(Encoding.UTF8);
                keyPair = new EcdsaKeyPair(keyType, comment, curve, publicKey, privateKey);
            }
            else if (keyType == KeyTypes.DSA)
            {
                var p = privateKeySectionReader.ReadBigInteger();
                var q = privateKeySectionReader.ReadBigInteger();
                var g = privateKeySectionReader.ReadBigInteger();
                var y = privateKeySectionReader.ReadBigInteger();
                var x = privateKeySectionReader.ReadBigInteger();
                var comment = privateKeySectionReader.ReadString(Encoding.UTF8);
                keyPair = new DsaKeyPair(comment, p, q, g, y, x);
            }
            else
            {
                throw new OpenSshKeyParseException($"key type '{keyType}' is not supported by this parser");
            }
            return keyPair;
        }
    }
}