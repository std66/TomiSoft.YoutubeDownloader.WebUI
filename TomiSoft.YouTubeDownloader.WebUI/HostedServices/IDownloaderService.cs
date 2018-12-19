using System;
using Tomisoft.YoutubeDownloader.Downloading;
using Tomisoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.WebUI.HostedServices {
    public interface IDownloaderService {
        Guid EnqueueDownload(string MediaUri, MediaFormat mediaFormat);
        DownloadProgress GetDownloadStatus(Guid DownloadID);
    }
}
