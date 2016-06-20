using DuplicateRemoverLib;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateRemoverCLI
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var controlledDirectory = new ControlledDirectory("TV", @"\\freenas\TV");

            Console.Write("Loading cache...");
            var loadResult = controlledDirectory.Load();
            Console.WriteLine((loadResult)? " Done" : " Failed");

            Console.Write("Scanning directory...");
            controlledDirectory.Update();
            Console.WriteLine(" Done {0} Files", controlledDirectory.RootNode.FilesRecursive.Count);

            Console.Write("Hashing files... ");
            controlledDirectory.Hash(1000);
            Console.WriteLine("Done");

            Console.Write("Saving cache... ");
            controlledDirectory.Save();
            Console.WriteLine("Done");

            Console.ReadLine();
        }
    }
}
