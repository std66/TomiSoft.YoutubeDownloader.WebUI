using System;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core {
    internal class QueuedDownload : IDisposable {
        public Guid DownloadID { get; }
        public IDownload DownloadHandler { get; }
        public DateTime QueueTimestamp { get; }
        public DateTime CompletedTimestamp { get; private set; }

        public event EventHandler<Guid> DownloadCompleted;

        public QueuedDownload(Guid DownloadId, IDownload DownloadProgress) {
            this.DownloadID = DownloadId;
            this.QueueTimestamp = DateTime.UtcNow;

            this.DownloadHandler = DownloadProgress;
            this.DownloadHandler.DownloadStatusChanged += DownloadProgress_DownloadStatusChanged;
        }

        private void DownloadProgress_DownloadStatusChanged(object sender, DownloadState e) {
            if (e == DownloadState.Completed || e == DownloadState.Failed) {
                this.CompletedTimestamp = DateTime.UtcNow;
                this.DownloadCompleted?.Invoke(this, DownloadID);
            }
        }

        public void Dispose() {
            this.DownloadHandler.DownloadStatusChanged -= DownloadProgress_DownloadStatusChanged;
            this.DownloadHandler.Dispose();
        }

        public override bool Equals(object obj) {
            return obj != null && (obj is QueuedDownload download) && download.DownloadID == this.DownloadID;
        }

        public override int GetHashCode() {
            return this.DownloadID.GetHashCode() * 7;
        }
    }
}
