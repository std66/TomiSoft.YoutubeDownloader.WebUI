using System;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.BusinessLogic.BusinessModels {
    public class DownloadRequestBM {
        public DownloadRequestBM(Uri mediaUri, MediaFormat format) {
            MediaUri = mediaUri;
            Format = format;
            DownloadId = Guid.NewGuid();
        }

        public Uri MediaUri { get; }
        public MediaFormat Format { get; }
        public Guid DownloadId { get; }
    }
}
