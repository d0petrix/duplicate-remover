using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleRemoverCLI
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var scanner = new FilesystemScanner();
            scanner.Scan(new System.IO.DirectoryInfo(@"\\freenas\TV"));

            Console.ReadLine();
        }
    }
}
