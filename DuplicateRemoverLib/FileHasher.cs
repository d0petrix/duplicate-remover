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

        public Hash Hash1KB(string filename)
        {
            try
            {
                using (var fileStream = File.OpenRead(filename))
                {
                    var buffer = new byte[1024];
                    var bytesRead = fileStream.Read(buffer, 0, 1024);
                    return new Hash(md5.ComputeHash(buffer, 0, bytesRead));
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
                return new Hash(md5.ComputeHash(fileStream));
            }
        }


    }
}
