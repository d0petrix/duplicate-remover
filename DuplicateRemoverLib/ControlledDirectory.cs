using Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateRemoverLib
{
    public class ControlledDirectory
    {
        public string Name { get; private set; }

        public string RootPath { get; private set; }

        public string CacheFilename { get; private set; }

        public DirectoryNode RootNode;

        public ControlledDirectory(string name, string rootPath)
        {
            Name = name;
            RootPath = rootPath;
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            CacheFilename = Path.Combine(appDataPath, "DuplicateRemover", Name + ".cache.gzip");
        }

        public void Update()
        {
            var scanner = new FilesystemScanner();
            var newRoot = scanner.Scan(RootPath);
            var filesystemCombiner = new FilesystemCombiner();
            var result = filesystemCombiner.Combine(RootNode, newRoot);
            RootNode = newRoot;
        }

        public void Load()
        {
            try
            {
                using (var stream = new GZipStream(new FileStream(CacheFilename, FileMode.Open, FileAccess.Read, FileShare.None), CompressionMode.Decompress))
                {
                    var formatter = new BinaryFormatter();
                    RootNode = (DirectoryNode)formatter.Deserialize(stream);
                }
            }
            catch (FileNotFoundException)
            {
                // nothing to load
            } 
        }

        public void Save()
        {
            IFormatter formatter = new BinaryFormatter();
            Directory.CreateDirectory(Path.GetDirectoryName(CacheFilename));
            using (var stream = new GZipStream(new FileStream(CacheFilename, FileMode.Create, FileAccess.Write, FileShare.None), CompressionLevel.Fastest))
            {
                formatter.Serialize(stream, RootNode);
                stream.Close();
            }

        }

        public void Hash(int count = 0)
        {
            var unhashedFiles = RootNode.FilesRecursive.Where(file => file.Hash1K == null).ToList();

            foreach (var file in unhashedFiles)
            {
                file.Calculate1kHash();

                if (count > 0)
                {
                    count--;
                    if (count == 0) return;
                }
            }
        }
    }
}
