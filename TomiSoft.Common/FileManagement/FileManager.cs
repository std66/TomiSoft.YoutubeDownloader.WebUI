using System;
using System.IO;
using TomiSoft.Common.FileManagement.Permissions;

namespace TomiSoft.Common.FileManagement {
    public class FileManager : IFileManager {
        
        public IFile GetFile(string Path) {
            return new File(Path);
        }

        public bool TryCreateFile(string Path, out IFile Result) {
            Result = null;

            IFile file = this.GetFile(Path);
            if (file.Exists)
                return false;

            try {
                System.IO.File.WriteAllText(Path, string.Empty);
                Result = new File(Path);
            }
            catch (Exception) {
                return false;
            }

            return true;
        }

        public bool TryCreateTextFile(string Path, string Content, FileCreationPermission CreationPermission, out IFile Result) {
            IFile file = this.GetFile(Path);
            if (file.Exists && CreationPermission == FileCreationPermission.AllowOverwrite)
                file.Delete();

            if (!this.TryCreateFile(Path, out Result))
                return false;

            try {
                using (TextWriter writer = Result.GetTextWriter()) {
                    writer.Write(Content);
                    writer.Flush();
                }
            }
            catch (Exception) {
                return false;
            }

            return true;
        }
    }
}
