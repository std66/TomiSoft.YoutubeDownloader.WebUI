using System.Collections.Generic;

namespace TomiSoft.YoutubeDownloader.Tests.Samples.SuccessfulAudioDownloadSamples {
    interface ISuccessfulAudioOutputSample : IAudioDownloadOutputSample {
        IEnumerable<string> GetStdOut();
    }
}