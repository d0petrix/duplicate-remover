using System;


namespace Nodes {

    [Serializable]
    public class FilesystemNode
    {
        public string Name;

        public DateTime CreationTime { get; private set; }
        public DateTime LastWriteTime { get; private set; }

        public DirectoryNode Parent { get; private set; }


        public string FullName
        {
            get
            {
                var fullName = this.ToString();
                FilesystemNode node = this;

                while (node.Parent != null)
                {
                    node = node.Parent;
                    fullName = node.ToString() + fullName;
                }

                return fullName;
            }
        }

        public FilesystemNode() { }

        public FilesystemNode(string name, DateTime creationTime, DateTime lastWriteTime, DirectoryNode parent = null)
        {
            Name = name;
            CreationTime = creationTime;
            LastWriteTime = lastWriteTime;
            Parent = parent;
        }

        protected void Update(FilesystemNode node)
        {
            CreationTime = node.CreationTime;
            LastWriteTime = node.LastWriteTime;
        }

    }

}
