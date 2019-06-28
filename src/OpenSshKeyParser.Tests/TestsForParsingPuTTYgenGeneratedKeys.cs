using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenSshKey.Parser.Tests
{
    [TestClass]
    public class TestsForParsingPuTTYgenGeneratedKeys
    {
        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_ED25519()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "ED25519.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data);
            Assert.IsTrue(keyPair != null && keyPair is Ed25519KeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_ED25519_Encrypted()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "ED25519.Encrypted.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data, "password");
            Assert.IsTrue(keyPair != null && keyPair is Ed25519KeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_RSA()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "RSA.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data);
            Assert.IsTrue(keyPair != null && keyPair is RsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_RSA_Encrypted()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "RSA.Encrypted.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data, "password");
            Assert.IsTrue(keyPair != null && keyPair is RsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_ECDSA_nistp256()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "ECDSA.nistp256.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data);
            Assert.IsTrue(keyPair != null && keyPair is EcdsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_ECDSA_nistp256_Encrypted()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "ECDSA.nistp256.Encrypted.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data, "password");
            Assert.IsTrue(keyPair != null && keyPair is EcdsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_ECDSA_nistp384()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "ECDSA.nistp384.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data);
            Assert.IsTrue(keyPair != null && keyPair is EcdsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_ECDSA_nistp384_Encrypted()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "ECDSA.nistp384.Encrypted.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data, "password");
            Assert.IsTrue(keyPair != null && keyPair is EcdsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_ECDSA_nistp521()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "ECDSA.nistp521.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data);
            Assert.IsTrue(keyPair != null && keyPair is EcdsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_ECDSA_nistp521_Encrypted()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "ECDSA.nistp521.Encrypted.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data, "password");
            Assert.IsTrue(keyPair != null && keyPair is EcdsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_DSA()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "DSA.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data);
            Assert.IsTrue(keyPair != null && keyPair is DsaKeyPair);
        }

        [TestMethod]
        public void OpenSshKeyParser_ParseOpenSshKeyFile_PuTTYgen_DSA_Encrypted()
        {
            var data = TestHelpers.GetResourceData("PuTTYgen", "DSA.Encrypted.txt");
            var keyPair = OpenSshKeyParser.ParseOpenSshKeyFile(data, "password");
            Assert.IsTrue(keyPair != null && keyPair is DsaKeyPair);
        }
    }
}
