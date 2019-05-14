
namespace OpenSshKey.Parser
{
    public class KeyPairBase : IKeyPair
    {
        public KeyPairBase(string keyType, string comment)
        {
            KeyType = keyType;
            Comment = comment;
        }

        public string KeyType { get; set; }

        public string Comment { get; set; }
    }
}
