using System;
using System.IO;

namespace TomiSoft.Common.FileManagement {
    public class File : IFile {
        public string Path { get; }

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

        public TextReader GetTextReader() {
            if (!this.Exists)
                throw new InvalidOperationException($"File cannot be opened for reading because it does not exists: '{this.Path}'");

            return System.IO.File.OpenText(this.Path);
        }

        public TextWriter GetTextWriter() {
            if (!this.Exists)
                throw new InvalidOperationException($"File cannot be opened for writing because it does not exists: '{this.Path}'");

            return new StreamWriter(System.IO.File.OpenWrite(Path));
        }

        public Stream Open(FileMode mode) {
            return System.IO.File.Open(this.Path, mode);
        }

        public File(string Path) {
            this.Path = Path;
        }
    }
}
