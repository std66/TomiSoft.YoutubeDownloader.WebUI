namespace TomiSoft.YoutubeDownloader.BusinessLogic.Configuration
{
    public interface IDownloaderServiceConfiguration {
        int MaximumParallelDownloads { get; }
        int DeleteFilesAfterMillisecondsElapsed { get; }
        string DownloadPath { get; }
    }
}
