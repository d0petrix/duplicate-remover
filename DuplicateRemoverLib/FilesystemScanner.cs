using DuplicateRemoverLib;
using log4net;
using Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FilesystemScanner
{
    private static readonly ILog log = LogManager.GetLogger(typeof(FilesystemScanner));

    private AbstractProgressManager progress;

    private double progressValue = 0.0;

    public FilesystemScanner(AbstractProgressManager progress)
    {
        this.progress = progress;
    }

    public DirectoryNode Scan(string dir)
    {
        progress.Start();
        var node = Scan(new DirectoryInfo(dir));
        progress.Stop();
        return node;
    }

    public DirectoryNode Scan(DirectoryInfo dir)
    {
         return ScanRecursive(dir, 1);
    }

    private DirectoryNode ScanRecursive(DirectoryInfo dir, double percentile, DirectoryNode parent = null)
    {
        var dirNode = new DirectoryNode(dir, parent);

        //log.Debug(dirNode.FullName);

        var directories = dir.GetDirectories().ToArray();
        foreach (var d in directories)
        {
            dirNode.Add(ScanRecursive(d, percentile / (double)directories.Length, dirNode));
        }

        if (directories.Length == 0)
        { 
            progressValue += percentile;
            progress.Update(progressValue, dir.Name);
        }

        foreach (var f in dir.GetFiles())
        {
            var fileNode = new FileNode(f, dirNode);
            //log.Debug(fileNode.FullName);
            dirNode.Add(fileNode);
        }

        return dirNode;        
    }

}

