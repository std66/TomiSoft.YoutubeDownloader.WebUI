namespace TomiSoft.YouTubeDownloader.WebUI.Metrics {
    public interface IMediaDownloadMetrics {
        void ReportFailedDownload();
        void ReportStartedDownload();
        void ReportSucceededDownload();
    }
}