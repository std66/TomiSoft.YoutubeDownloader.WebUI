namespace TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core {
    public interface IDownloaderServiceConfiguration {
        int MaximumParallelDownloads { get; }
        int DeleteFilesAfterMillisecondsElapsed { get; }
    }
}
