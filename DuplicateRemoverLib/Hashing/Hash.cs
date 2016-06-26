using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateRemoverLib.Hashing
{
    [Serializable]
    public class Hash
    {
        public byte[] Value { get; private set; }
        public long ChunkSize { get; private set; }

        public Hash(byte[] value, long chunkSize)
        {
            Value = value;
            ChunkSize = chunkSize;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(Value, 0);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Hash;
            if (other == null) return false;
            if (other.Value.Length != Value.Length) return false;

            for (var i = 0; i < Value.Length; i++)
            {
                if (Value[i] != other.Value[i]) return false;
            }

            return true;
        }

        public override string ToString()
        {
            return ByteArrayToString(Value);
        }
    }
}
