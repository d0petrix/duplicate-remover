using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateRemoverLib
{
    public class FilesystemScannerProgress 
    {

        public int FilesCount;
        public int FilesDone;
        public int DirectoriesCount;
        public int DirectoriesDone;

        public double Value
        {
            get
            {
                return ((double)FilesCount + (double)DirectoriesCount) / ((double)FilesDone + (double) DirectoriesDone);
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
