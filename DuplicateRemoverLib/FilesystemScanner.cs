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

    public DirectoryNode Scan(DirectoryInfo dir)
    {
        return ScanRecursive(dir);
    }

    private DirectoryNode ScanRecursive(DirectoryInfo dir, DirectoryNode parent = null)
    {
        var dirNode = new DirectoryNode(dir, parent);

        log.Debug(dirNode.FullName);

        foreach (var d in dir.GetDirectories())
        {
            dirNode.Add(ScanRecursive(d, dirNode));
        }

        foreach (var f in dir.GetFiles())
        {
            var fileNode = new FileNode(f, dirNode);
            log.Debug(fileNode.FullName);
            fileNode.CalculateHash();
            dirNode.Add(fileNode);
        }

        return dirNode;        
    }

}

