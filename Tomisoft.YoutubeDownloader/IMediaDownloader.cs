using System;
using Tomisoft.YoutubeDownloader.Downloading;
using Tomisoft.YoutubeDownloader.Media;

namespace Tomisoft.YoutubeDownloader {
    public interface IMediaDownloader {
        IMediaInformation GetMediaInformation(Uri MediaUri);
        string GetVersion();
        IDownload PrepareDownload(Uri MediaUri, MediaFormat MediaFormat);
    }
}