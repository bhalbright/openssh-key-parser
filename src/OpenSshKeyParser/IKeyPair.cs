
namespace OpenSshKey.Parser
{
    public interface IKeyPair
    {
        string KeyType { get; }

        string Comment { get; }
    }
}
