using System;

namespace OpenSshKey.Parser
{
    public class OpenSshKeyParseException : Exception
    {
        public OpenSshKeyParseException(string message)
            : base(message)
        {
        }

        public OpenSshKeyParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
