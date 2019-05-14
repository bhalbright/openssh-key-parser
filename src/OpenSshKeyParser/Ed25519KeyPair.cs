
namespace OpenSshKey.Parser
{
    public class Ed25519KeyPair : KeyPairBase
    {
        public Ed25519KeyPair(string comment, byte[] publicKey, byte[] privateKey) : base(KeyTypes.ED25519, comment)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public byte[] PublicKey { get; }

        public byte[] PrivateKey { get; }
    }
}
