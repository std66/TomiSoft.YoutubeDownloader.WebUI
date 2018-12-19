using System;
using Tomisoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
    internal class QueuedDownload : IDisposable {
        public Guid DownloadID { get; }
        public DownloadProgress DownloadProgress { get; }
        public DateTime QueueTimestamp { get; }
        public DateTime CompletedTimestamp { get; private set; }

        public event EventHandler<Guid> DownloadCompleted;

        public QueuedDownload(Guid DownloadId, DownloadProgress DownloadProgress) {
            this.DownloadID = DownloadId;
            this.QueueTimestamp = DateTime.UtcNow;

            this.DownloadProgress = DownloadProgress;
            this.DownloadProgress.DownloadStatusChanged += DownloadProgress_DownloadStatusChanged;
        }

        private void DownloadProgress_DownloadStatusChanged(object sender, DownloadState e) {
            if (e == DownloadState.Completed || e == DownloadState.Failed) {
                this.CompletedTimestamp = DateTime.UtcNow;
                this.DownloadCompleted?.Invoke(this, DownloadID);
            }
        }

        public void Dispose() {
            this.DownloadProgress.DownloadStatusChanged -= DownloadProgress_DownloadStatusChanged;
            this.DownloadProgress.Dispose();
        }

        public override bool Equals(object obj) {
            return obj != null && (obj is QueuedDownload download) && download.DownloadID == this.DownloadID;
        }

        public override int GetHashCode() {
            return this.DownloadID.GetHashCode() * 7;
        }
    }
}
