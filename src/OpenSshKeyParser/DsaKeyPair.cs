using System.Numerics;

namespace OpenSshKey.Parser
{
    public class DsaKeyPair : KeyPairBase
    {
        public DsaKeyPair(string comment, BigInteger p, BigInteger q, BigInteger g,
            BigInteger y, BigInteger x) : base(KeyTypes.DSA, comment)
        {
            P = p;
            Q = q;
            G = g;
            Y = y;
            X = x;
        }

        /// <summary>
        /// Prime 1
        /// </summary>
        public BigInteger P { get; }

        /// <summary>
        /// Prime 2
        /// </summary>
        public BigInteger Q { get; }

        /// <summary>
        /// </summary>
        public BigInteger G { get; }

        /// <summary>
        /// </summary>
        public BigInteger Y { get; }

        /// <summary>
        /// </summary>
        public BigInteger X { get; }
    }
}
