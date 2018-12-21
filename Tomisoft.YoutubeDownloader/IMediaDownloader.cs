using System;
using Tomisoft.YoutubeDownloader.Downloading;
using Tomisoft.YoutubeDownloader.Media;

namespace Tomisoft.YoutubeDownloader {
    public interface IMediaDownloader {
        IMediaInformation GetMediaInformation(Uri MediaUri);
        string GetVersion();
        DownloadProgress PrepareDownload(Uri MediaUri, MediaFormat MediaFormat);
    }
}