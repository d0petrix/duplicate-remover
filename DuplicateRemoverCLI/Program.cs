using DuplicateRemover;
using DuplicateRemoverLib;
using log4net;
using Microsoft.GotDotNet;
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

            var consoleProgress = new ConsoleProgressManager();
            var controlledDirectory = new ControlledDirectory("TV", @"\\freenas\TV");
            controlledDirectory.Progress = consoleProgress;

            Console.Write("Loading cache...");
            var loadResult = controlledDirectory.Load();
            if (loadResult)
                Console.WriteLine(" Done {1} Directories {0} Files", controlledDirectory.RootNode.FilesRecursive.Count, controlledDirectory.RootNode.DirectoriesRecursive.Count);
            else
                Console.WriteLine(" Failed");

            Console.Write("Scanning directory...");
            controlledDirectory.Update();
            Console.WriteLine(" Done {1} Directories {0} Files", controlledDirectory.RootNode.FilesRecursive.Count, controlledDirectory.RootNode.DirectoriesRecursive.Count);

            Console.Write("Hashing files... ");
            consoleProgress.SavePosition();
            controlledDirectory.Hash();
            Console.WriteLine();

            Console.Write("Saving cache... ");
            controlledDirectory.Save();
            Console.WriteLine("Done");

            var test = controlledDirectory.FindDuplicates();

            Console.ReadLine();
        }
    }
}
