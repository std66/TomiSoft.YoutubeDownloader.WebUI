using System;

namespace TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core.ServiceState {
    public interface IDownloaderServiceState {
        DateTime LastUpdateTime {
            get;
            set;
        }
    }
}
