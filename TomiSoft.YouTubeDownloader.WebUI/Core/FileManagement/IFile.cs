namespace TomiSoft.YouTubeDownloader.WebUI.Core.FileManagement {
    public interface IFile {
        bool Exists { get; }
        bool Delete();
    }
}
