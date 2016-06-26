using DuplicateRemover;
using DuplicateRemoverLib;
using log4net;
using Microsoft.GotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DuplicateRemoverCLI
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(
        long fileSize
        , [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer
        , int bufferSize);

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
            consoleProgress.SavePosition();
            controlledDirectory.Update();
            Console.WriteLine(" Done {1} Directories {0} Files", controlledDirectory.RootNode.FilesRecursive.Count, controlledDirectory.RootNode.DirectoriesRecursive.Count);

            Console.Write("Hashing files... ");
            consoleProgress.SavePosition();
            controlledDirectory.Hash();
            Console.WriteLine();

            Console.Write("Saving cache... ");
            controlledDirectory.Save();
            Console.WriteLine("Done");

            Console.Write("Finding duplicates... ");
            consoleProgress.SavePosition();
            var duplicates = controlledDirectory.FindDuplicates();
            Console.WriteLine("Done");

            Console.WriteLine();

            decimal deleteSum = 0;

            Console.WriteLine("Interactive duplicate deletion");
            foreach (var duplicate in duplicates)
            {
                var keepFile = duplicate.OrderBy(d => d.CreationTime).FirstOrDefault();
                
                foreach (var file in duplicate.OrderByDescending(d => d.CreationTime))
                {
                    var size = StrFormatByteSize(file.Length).PadLeft(8);
                    Console.WriteLine("{0} {1} - {2:d} - {3}", (keepFile == file) ? ">": "D", size, file.CreationTime, file.Name);
                }

                duplicate.Remove(keepFile);

                decimal deleteSize = duplicate.Sum(d => d.Length);
                deleteSum += deleteSize;

                Console.WriteLine("(D)elete {0}?", StrFormatByteSize((long)deleteSize));

                //var size = StrFormatByteSize(keepFile.Length).PadLeft(8);
                //Console.WriteLine("{0} {1} - {2:d} - {3}", ">", size, keepFile.CreationTime, keepFile.Name);

                switch (Console.ReadLine())
                {
                    case "D":
                        foreach (var file in duplicate)
                        {
                            try {
                                System.IO.File.Delete(file.FullName);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                        break;
                }
            }

            Console.ReadLine();
        }

        public static string StrFormatByteSize(long filesize)
        {
            StringBuilder sb = new StringBuilder(11);
            StrFormatByteSize(filesize, sb, sb.Capacity);
            return sb.ToString();
        }
    }
}
