namespace TomiSoft.YoutubeDownloader.BusinessLogic.Configuration {
    public interface IDownloaderServiceConfiguration {
        int DeleteFilesAfterMinutesElapsed { get; }
        string DownloadPath { get; }
    }
}
