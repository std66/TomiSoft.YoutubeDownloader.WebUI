using System;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.BackgroundDownloaderService.HostedServices {
    public interface IDownloaderService {
        Guid EnqueueDownload(Uri MediaUri, MediaFormat mediaFormat);
        IDownload GetDownloadStatus(Guid DownloadID);
    }
}
