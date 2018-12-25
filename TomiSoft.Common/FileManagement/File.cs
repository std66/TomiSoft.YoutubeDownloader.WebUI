using System;
using System.IO;

namespace TomiSoft.Common.FileManagement {
    public class File : IFile {
        private readonly string Path;

        public bool Exists => System.IO.File.Exists(Path);

        public string DirectoryName => System.IO.Path.GetDirectoryName(this.Path);

        public bool Delete() {
            try {
                System.IO.File.Delete(Path);
            }
            catch (IOException) {
                return false;
            }

            return true;
        }

        public IFile Rename(string NewName) {
            if (!this.Exists)
                throw new InvalidOperationException($"File cannot be renamed because it does not exists: '{this.Path}'");

            if (!System.IO.Path.IsPathRooted(NewName))
                NewName = System.IO.Path.Combine(this.DirectoryName, NewName);

            System.IO.File.Move(
                sourceFileName: this.Path,
                destFileName: NewName
            );

            return new File(NewName);
        }

        public File(string Path) {
            this.Path = Path;
        }
    }
}
