using DuplicateRemoverLib.Hashing;
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

        public Hash HashPart(string filename, int length)
        {
            try
            {
                using (var fileStream = File.OpenRead(filename))
                {
                    var buffer = new byte[length];
                    var bytesRead = fileStream.Read(buffer, 0, length);
                    return new Hash(md5.ComputeHash(buffer, 0, bytesRead), length);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Hash Hash(string filename)
        {
            using (var fileStream = File.OpenRead(filename))
            {
                var length = fileStream.Length;
                return new Hash(md5.ComputeHash(fileStream), length);
            }
        }


    }
}
