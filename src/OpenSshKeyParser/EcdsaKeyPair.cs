
namespace OpenSshKey.Parser
{
    public class EcdsaKeyPair : KeyPairBase
    {
        public EcdsaKeyPair(string keyType, string comment, 
            string curve, byte[] publicKey, byte[] privateKey) : base(keyType, comment)
        {
            KeyType = keyType;
            Curve = curve;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public string Curve { get; }

        public byte[] PublicKey { get; }

        public byte[] PrivateKey { get; }
    }
}
