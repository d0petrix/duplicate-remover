using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateRemoverLib
{
    public class ControlledDirectory
    {
        public string Name { get; private set; }

        public string RootPath { get; private set; }

        public string CacheFilename { get; private set; }

        public ControlledDirectory RootNode;

        public ControlledDirectory(string name, string rootPath)
        {
            RootPath = rootPath;
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            CacheFilename = Path.Combine(appDataPath, "Cache", Name + ".cache");
        }

        public void Load()
        {

        }
    }
}
