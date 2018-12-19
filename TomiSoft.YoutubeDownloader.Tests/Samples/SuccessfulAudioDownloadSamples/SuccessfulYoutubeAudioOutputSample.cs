using System.Collections.Generic;
using Tomisoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.Tests.Samples.SuccessfulAudioDownloadSamples {
    class SuccessfulYoutubeAudioOutputSample : ISuccessfulAudioOutputSample {
        public IEnumerable<string> GetStdOut() {
            return new List<string>() {
                "[youtube] 9nGO8oTY1xI: Downloading webpage",
                "[youtube] 9nGO8oTY1xI: Downloading video info webpage",
                "[download] Destination: Peat Jr. & Fernando - Itt a nyár [dalszöveg]-9nGO8oTY1xI.webm",
                "[download]   0.0% of 3.01MiB at 166.67KiB/s ETA 00:18",
                "[download]   0.1% of 3.01MiB at 500.00KiB/s ETA 00:06",
                "[download]   0.2% of 3.01MiB at  1.14MiB/s ETA 00:02",
                "[download]   0.5% of 3.01MiB at  2.09MiB/s ETA 00:01",
                "[download]   1.0% of 3.01MiB at  1.32MiB/s ETA 00:02",
                "[download]   2.0% of 3.01MiB at  1.50MiB/s ETA 00:01",
                "[download]   4.1% of 3.01MiB at  1.94MiB/s ETA 00:01",
                "[download]   8.3% of 3.01MiB at  2.26MiB/s ETA 00:01",
                "[download]  16.6% of 3.01MiB at  2.45MiB/s ETA 00:01",
                "[download]  33.2% of 3.01MiB at  2.56MiB/s ETA 00:00",
                "[download]  66.4% of 3.01MiB at  2.64MiB/s ETA 00:00",
                "[download] 100.0% of 3.01MiB at  2.66MiB/s ETA 00:00",
                "[download] 100% of 3.01MiB in 00:01",
                "[ffmpeg] Destination: Peat Jr. & Fernando - Itt a nyár [dalszöveg]-9nGO8oTY1xI.mp3",
                "Deleting original file Peat Jr. & Fernando - Itt a nyár [dalszöveg]-9nGO8oTY1xI.webm (pass -k to keep)",
                ""
            };
        }

        public string MediaUri => "https://www.youtube.com/watch?v=9nGO8oTY1xI";

        public IEnumerable<DownloadState> ExpectedDownloadStatuses => new DownloadState[] {
            DownloadState.Starting,
            DownloadState.Downloading,
            DownloadState.PostProcessing,
            DownloadState.Completed
        };

        public IEnumerable<double> ExpectedPercents => new double[] {
            0.0,
            0.1,
            0.2,
            0.5,
            1.0,
            2.0,
            4.1,
            8.3,
            16.6,
            33.2,
            66.4,
            100.0
        };
    }
}
