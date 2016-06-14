using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateRemover
{
    public class FilesystemCombiner
    {

        public DirectoryNode Combine(DirectoryNode oldRoot, DirectoryNode newRoot) {

            var filesToRemove = new List<FileNode>();

            var synchronisationResult = new SynchronisationResult();

            foreach (var oldFile in oldRoot.Files)
            {
                var newFile = newRoot.Files.Where(file => file.Name == oldFile.Name).FirstOrDefault();

                if (newFile == null)
                {
                    filesToRemove.Add(oldFile);
                    synchronisationResult.RemovedNodes.Add(oldFile);
                }
                else if (oldFile.LastWriteTime != newFile.LastWriteTime)
                {
                    oldFile.Update(newFile);
                    synchronisationResult.ModifiedNodes.Add(oldFile);
                }
            }



            return oldRoot;
        }

    }
}
