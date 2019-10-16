using System;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels {
    public class CompletedQueuedDownloadBM : QueuedDownloadBM {
        public CompletedQueuedDownloadBM(Guid downloadId, IDownload downloadProgress, DateTime completionTimestampUtc)
             : base(downloadId, downloadProgress) {
            CompletionTimestampUtc = completionTimestampUtc;
        }

        public DateTime CompletionTimestampUtc { get; }
    }
}
