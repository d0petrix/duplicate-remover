using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FilesystemCombiner
{

    public SynchronisationResult Combine(DirectoryNode oldRoot, DirectoryNode newRoot)
    {
        var synchronisationResult = new SynchronisationResult();

        Combine(oldRoot, newRoot, synchronisationResult);

        return synchronisationResult;
    }

    private void Combine(DirectoryNode oldRoot, DirectoryNode newRoot, SynchronisationResult synchronisationResult)
    {
        if (oldRoot == null)
        {
            // current subtree is completely new
            synchronisationResult.AddedNodes.AddRange(FlattenToNodeList(newRoot));
            return;
        }

        foreach (var newFile in newRoot.Files)
        {
            var oldFile = oldRoot.Files.FirstOrDefault(file => file.Name == newFile.Name);

            if (oldFile != null)
            {
                // old node exists
                if (oldFile.Length == newFile.Length && oldFile.LastWriteTime == newFile.LastWriteTime)
                    // reuse hash from old node
                    newFile.SmallHash = oldFile.SmallHash;
                else
                    // file was modified
                    synchronisationResult.ModifiedNodes.Add(newFile);
            }
            else
                synchronisationResult.AddedNodes.Add(newFile);            
        }

        foreach (var newDir in newRoot.Directories)
        {
            var oldDir = oldRoot.Directories.FirstOrDefault(dir => dir.Name == newDir.Name);

            Combine(oldDir, newDir, synchronisationResult);
        }
    }

    private List<FilesystemNode> FlattenToNodeList(DirectoryNode root, List<FilesystemNode> nodeList = null)
    {
        if (nodeList == null) nodeList = new List<FilesystemNode>();
        if (root == null) return nodeList;

        nodeList.Add(root);

        foreach (var fileNode in root.Files)
        {
            nodeList.Add(fileNode);
        }

        foreach (var dirNode in root.Directories)
        {
            FlattenToNodeList(dirNode, nodeList);
        }

        return nodeList;
    }

}