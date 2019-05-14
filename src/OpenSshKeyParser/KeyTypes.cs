
namespace OpenSshKey.Parser
{
    static class KeyTypes
    {
        public const string ED25519 = "ssh-ed25519";
        public const string RSA = "ssh-rsa";
        public const string DSA = "ssh-dss";
        public const string ECDSA256 = "ecdsa-sha2-nistp256";
        public const string ECDSA384 = "ecdsa-sha2-nistp384";
        public const string ECDSA521 = "ecdsa-sha2-nistp521";
    }
}
