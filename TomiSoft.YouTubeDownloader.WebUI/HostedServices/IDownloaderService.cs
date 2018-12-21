using System;
using Tomisoft.YoutubeDownloader.Downloading;
using Tomisoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.WebUI.HostedServices {
    public interface IDownloaderService {
        Guid EnqueueDownload(Uri MediaUri, MediaFormat mediaFormat);
        IDownload GetDownloadStatus(Guid DownloadID);
    }
}
