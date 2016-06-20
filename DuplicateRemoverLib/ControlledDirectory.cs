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

        public string CacheFilename {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(RootPath, "DuplicateRemover.cache.gzip");
            }
        }

        private FileStream cacheFile;

        public DirectoryNode RootNode;

        public ControlledDirectory(string name, string rootPath)
        {
            Name = name;
            RootPath = rootPath;
            cacheFile = new FileStream(CacheFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }

        public void Update()
        {
            var scanner = new FilesystemScanner();
            var newRoot = scanner.Scan(RootPath);
            var filesystemCombiner = new FilesystemCombiner();
            var result = filesystemCombiner.Combine(RootNode, newRoot);
            RootNode = newRoot;
        }

        public bool Load()
        {
            // New file
            if (cacheFile.Length <= 0) return false;

            cacheFile.Seek(0, SeekOrigin.Begin);

            using (var stream = new GZipStream(cacheFile, CompressionMode.Decompress))
            {
                var formatter = new BinaryFormatter();
                RootNode = (DirectoryNode)formatter.Deserialize(stream);
            }

            return true;
        }

        public void Save()
        {
            cacheFile.Seek(0, SeekOrigin.Begin);
            IFormatter formatter = new BinaryFormatter();
            Directory.CreateDirectory(Path.GetDirectoryName(CacheFilename));
            using (var stream = new GZipStream(cacheFile, CompressionLevel.Fastest))
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
