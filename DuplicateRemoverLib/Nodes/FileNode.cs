using DuplicateRemover;
using DuplicateRemoverLib.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes
{
    [Serializable]
    public class FileNode : FilesystemNode
    {

        public long Length { get; set; }
        public Hash SmallHash { get; set; }
        public Hash FullHash { get; set; }

        public FileNode(FileInfo fileInfo, DirectoryNode parent) : base(fileInfo.Name, fileInfo.CreationTime, fileInfo.LastWriteTime, parent)
        {
            Length = fileInfo.Length;
        }

        public void CalculateSmallHash()
        {
            var fileHash = new FileHasher();
            SmallHash = fileHash.HashPart(FullName, 1024 * 4);
        }

        public void Update(FileNode node)
        {
            Update(node);
            Length = node.Length;
            SmallHash = node.SmallHash;
        }

        public override string ToString()
        {
            return Name;
        }

        internal void CalculateFullHash()
        {
            var fileHash = new FileHasher();
            FullHash = fileHash.Hash(FullName);
        }
    }
}
