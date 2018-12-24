using System.IO;

namespace TomiSoft.YouTubeDownloader.WebUI.Core.FileManagement {
    public class File : IFile {
        private readonly string Path;

        public bool Exists => System.IO.File.Exists(Path);

        public bool Delete() {
            try {
                System.IO.File.Delete(Path);
            }
            catch (IOException) {
                return false;
            }

            return true;
        }

        public File(string Path) {
            this.Path = Path;
        }
    }
}
