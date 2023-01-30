using System;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YouTubeDownloader.WebUI.Metrics {
    public class DownloadMetricsDecorator : IDownload {
        private readonly IDownload _download;
        private readonly IMediaDownloadMetrics metrics;

        public DownloadMetricsDecorator(IDownload download, IMediaDownloadMetrics metrics) {
            _download = download;
            this.metrics = metrics;

            this._download.DownloadStatusChanged += ReportMetricsOnDownloadStatusChanged;
        }

        private void ReportMetricsOnDownloadStatusChanged(object sender, DownloadState e) {
            switch (e) {
                case DownloadState.Starting: metrics.ReportStartedDownload(); break;
                case DownloadState.Completed: metrics.ReportSucceededDownload(); break;
                case DownloadState.Failed: metrics.ReportFailedDownload(); break;
            }
        }

        public string Filename => _download.Filename;

        public double Percentage => _download.Percentage;

        public string ErrorMessage => _download.ErrorMessage;

        public string CommandLine => _download.CommandLine;

        public DownloadState Status => _download.Status;

        public event EventHandler<DownloadState> DownloadStatusChanged {
            add {
                _download.DownloadStatusChanged += value;
            }

            remove {
                _download.DownloadStatusChanged -= value;
            }
        }

        public event EventHandler<double> PercentageChanged {
            add {
                _download.PercentageChanged += value;
            }

            remove {
                _download.PercentageChanged -= value;
            }
        }

        public void Dispose() {
            this._download.DownloadStatusChanged -= ReportMetricsOnDownloadStatusChanged;
            _download.Dispose();
        }

        public void Start() {
            _download.Start();
        }
    }
}
