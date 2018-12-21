using System;

namespace Tomisoft.YoutubeDownloader.Downloading {
    public interface IDownload : IDisposable {
        string Filename { get; }
        double Percentage { get; }
        DownloadState Status { get; }

        event EventHandler<DownloadState> DownloadStatusChanged;
        event EventHandler<double> PercentageChanged;

        void Start();
    }
}