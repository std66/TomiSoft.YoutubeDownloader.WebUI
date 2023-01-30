using Prometheus;

namespace TomiSoft.YouTubeDownloader.WebUI.Metrics {
    public class MediaDownloadMetrics : IMediaDownloadMetrics {
        private static readonly Counter StartedDownloads = Prometheus.Metrics.CreateCounter("downloads_started", "Number of started downloads.");
        private static readonly Counter SuccessfulDownloads = Prometheus.Metrics.CreateCounter("downloads_successful", "Number of successful downloads.");
        private static readonly Counter FailedDownloads = Prometheus.Metrics.CreateCounter("downloads_failed", "Number of failed downloads.");
        private static readonly Gauge DownloadsInFlight = Prometheus.Metrics.CreateGauge("downloads_active", "Number of active downloads.");

        public void ReportStartedDownload() {
            StartedDownloads.Inc();
            DownloadsInFlight.Inc();
        }

        public void ReportSucceededDownload() {
            SuccessfulDownloads.Inc();
            DownloadsInFlight.Dec();
        }

        public void ReportFailedDownload() {
            FailedDownloads.Inc();
            DownloadsInFlight.Dec();
        }
    }
}
