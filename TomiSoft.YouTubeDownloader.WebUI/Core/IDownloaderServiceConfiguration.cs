namespace TomiSoft.YouTubeDownloader.WebUI.Core {
    public interface IDownloaderServiceConfiguration {
        int MaximumParallelDownloads { get; }
        int DeleteFilesAfterMillisecondsElapsed { get; }
    }
}
