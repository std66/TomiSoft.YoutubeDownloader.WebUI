namespace TomiSoft.YouTubeDownloader.WebUI.Core.FileManagement {
    public class FileManager : IFileManager {
        public IFile GetFile(string Path) {
            return new File(Path);
        }
    }
}
