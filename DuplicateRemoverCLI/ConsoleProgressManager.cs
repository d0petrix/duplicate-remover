using DuplicateRemoverLib;
using Microsoft.GotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DuplicateRemover
{
    public class ConsoleProgressManager : AbstractProgressManager
    {
        private int x;
        private int y;
        private Task task;

        private bool running = false;

        private string lastStatus;

        public void SavePosition()
        {
            x = ConsoleEx.CursorX;
            y = ConsoleEx.CursorY;
        }

        public override void Start()
        {
            running = true;
            task = Task.Factory.StartNew(() =>
            {

                do
                {
                    PrintToConsole();
                    Thread.Sleep(50);
                } while (running);
                               

            });
        }

        public override void Stop()
        {
            running = false;
            Value = 1;
            Status = string.Empty;
            PrintToConsole();
        }

        private void PrintToConsole()
        {
            var text = string.Format("{0,3:##0.}% {1}", (Value * 100.0), Status);

            if (lastStatus != null && Status != null && lastStatus != Status && lastStatus.Length > Status.Length)
            {
                text += new string(' ', lastStatus.Length - Status.Length);
                //ConsoleEx.WriteAt(x + Status.Length + 5, y, new String(' ', lastStatus.Length - Status.Length));
            }

            lastStatus = Status;
            ConsoleEx.WriteAt(x, y, text);
        }
    }
}
