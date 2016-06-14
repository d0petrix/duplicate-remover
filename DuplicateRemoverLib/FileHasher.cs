using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateRemover
{
    public class FileHasher
    {
        private ASCIIEncoding enc = new ASCIIEncoding();
        private MD5 md5 = MD5.Create();

        public string Hash1KB(string filename)
        {
            try
            {
                using (var fileStream = File.OpenRead(filename))
                {
                    var buffer = new byte[1024];
                    var bytesRead = fileStream.Read(buffer, 0, 1024);
                    var hash = md5.ComputeHash(buffer, 0, bytesRead);
                    //var hashString = new string(enc.GetChars(hash));
                    return ByteArrayToString(hash);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Hash(string filename)
        {
            using (var fileStream = File.OpenRead(filename))
            {
                var hash = md5.ComputeHash(fileStream);
                var hashString = new string(enc.GetChars(hash));
                return hashString;
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
