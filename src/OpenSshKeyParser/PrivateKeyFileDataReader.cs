using System;
using System.IO;
using System.Numerics;
using System.Text;

/// <summary>
/// Acknowledgement: this code is mostly small sub-set of the SshData class in https://github.com/sshnet/SSH.NET. (MIT licensed) 
/// </summary>
namespace OpenSshKey.Parser
{
    class PrivateKeyFileDataReader : IDisposable
    {
        private MemoryStream _stream;

        public PrivateKeyFileDataReader(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("data cannot be null or empty", "data");
            }
            _stream = new MemoryStream(data, 0, data.Length);
        }

        public string ReadString(Encoding encoding)
        {
            var length = ReadInt32();

            if (length > int.MaxValue)
            {
                throw new NotSupportedException($"cannot read string longer than {int.MaxValue}");
            }

            var bytes = ReadBytes((int)length);
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        public byte[] ReadBytes()
        {
            var bytesLength = (int)(_stream.Length - _stream.Position);
            var data = new byte[bytesLength];
            _stream.Read(data, 0, bytesLength);
            return data;
        }

        public byte[] ReadBytes(int length)
        {
            var data = new byte[length];
            var bytesRead = _stream.Read(data, 0, length);

            if (bytesRead < length)
                throw new ArgumentOutOfRangeException("length");

            return data;
        }

        public int ReadInt32()
        {
            var buffer = ReadBytes(4);
            var uintValue = (uint)buffer[0] << 24 |
                   (uint)buffer[1] << 16 |
                   (uint)buffer[2] << 8 |
                   buffer[3];
            return (int)uintValue;

        }

        public BigInteger ReadBigInteger()
        {
            var length = ReadInt32();
            var data = ReadBytes(length);
            return new BigInteger(data);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _stream.Dispose();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
