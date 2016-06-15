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

            controlledDirectory.Update();

            controlledDirectory.Save();

            Console.ReadLine();
        }
    }
}
