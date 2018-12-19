using System.Collections.Generic;
using Tomisoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.Tests.Samples {
    interface  IAudioDownloadOutputSample {
        IEnumerable<DownloadState> ExpectedDownloadStatuses { get; }
        IEnumerable<double> ExpectedPercents { get; }
        string MediaUri { get; }
    }
}
