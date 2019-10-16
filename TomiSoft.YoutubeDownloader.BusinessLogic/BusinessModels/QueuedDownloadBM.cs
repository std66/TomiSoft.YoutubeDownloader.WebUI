using System;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels {
    public class QueuedDownloadBM {
        public Guid DownloadID { get; }
        public IDownload DownloadHandler { get; }

        public QueuedDownloadBM(Guid downloadId, IDownload downloadProgress) {
            this.DownloadID = downloadId;

            this.DownloadHandler = downloadProgress;
        }
    }
}
