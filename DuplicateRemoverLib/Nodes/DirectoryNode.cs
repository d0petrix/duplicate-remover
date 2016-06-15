﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nodes
{
    [Serializable]
    public class DirectoryNode : FilesystemNode
    {
        private List<FilesystemNode> children;

        public FilesystemNode[] Children { get { return children.ToArray(); } }
        
        /// <summary>
        /// All files in this directory
        /// </summary>
        public List<FileNode> Files { get { return Children.OfType<FileNode>().ToList(); } }

        /// <summary>
        /// All directories in this directory
        /// </summary>
        public List<DirectoryNode> Directories { get { return Children.OfType<DirectoryNode>().ToList(); } }

        public string BasePath { get; private set; }


        public DirectoryNode() { }

        public DirectoryNode(DirectoryInfo dirInfo, DirectoryNode parent = null) : base(dirInfo.Name, dirInfo.CreationTime, dirInfo.LastWriteTime, parent)
        {
            children = new List<FilesystemNode>();
            if (parent == null)
                BasePath = dirInfo.FullName;
        }

        public void Add(FilesystemNode node)
        {
            children.Add(node);
        }

        public override string ToString()
        {
            return ((BasePath == null) ? Name : BasePath) + @"\";
        }

    }
}
