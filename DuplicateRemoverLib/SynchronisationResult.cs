﻿using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SynchronisationResult
{

    public List<FilesystemNode> RemovedNodes { get; private set; }

    public List<FilesystemNode> AddedNodes { get; private set; }

    public List<FilesystemNode> ModifiedNodes { get; private set; }

    public DirectoryNode Root { get; internal set; }

    public SynchronisationResult()
    {
        RemovedNodes = new List<FilesystemNode>();
        AddedNodes = new List<FilesystemNode>();
        ModifiedNodes = new List<FilesystemNode>();
    }

}
