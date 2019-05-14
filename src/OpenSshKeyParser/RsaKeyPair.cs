using System.Numerics;

namespace OpenSshKey.Parser
{
    public class RsaKeyPair : KeyPairBase
    {
        public RsaKeyPair(string comment, BigInteger n, BigInteger e, BigInteger d, 
            BigInteger iqmp, BigInteger p, BigInteger q) : base(KeyTypes.RSA, comment)
        {
            N = n;
            E = e;
            D = d;
            Iqmp = iqmp;
            P = p;
            Q = q;
        }

        /// <summary>
        /// Modulus
        /// </summary>
        public BigInteger N { get; }

        /// <summary>
        /// Public exponent
        /// </summary>
        public BigInteger E { get; }

        /// <summary>
        /// Private exponent
        /// </summary>
        public BigInteger D { get; }

        /// <summary>
        /// //(q ^ -1 mod p) i.e. inverse q mod p
        /// </summary>
        public BigInteger Iqmp { get; }

        /// <summary>
        /// Prime 1
        /// </summary>
        public BigInteger P { get; }

        /// <summary>
        /// Prime 2
        /// </summary>
        public BigInteger Q { get; }        
    }
}