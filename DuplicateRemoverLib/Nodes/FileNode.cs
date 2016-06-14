using DoubleRemover;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes
{
    public class FileNode : FilesystemNode
    {

        public long Length { get; set; }
        public string Hash1K { get; set; }

        public FileNode(FileInfo fileInfo, DirectoryNode parent) : base(fileInfo.Name, fileInfo.CreationTime, fileInfo.LastWriteTime, parent)
        {
            Length = fileInfo.Length;
        }

        public void CalculateHash()
        {
            var fileHash = new FileHasher();
            Hash1K = fileHash.Hash1KB(FullName);
            Console.WriteLine(Hash1K);
        }

        public void Update(FileNode node)
        {
            Update(node);
            Length = node.Length;
            Hash1K = node.Hash1K;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
