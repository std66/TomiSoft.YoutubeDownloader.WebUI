namespace TomiSoft.YoutubeDownloader.BusinessLogic.Configuration
{
    public interface IDownloaderServiceConfiguration {
        int DeleteFilesAfterMillisecondsElapsed { get; }
        string DownloadPath { get; }
    }
}
