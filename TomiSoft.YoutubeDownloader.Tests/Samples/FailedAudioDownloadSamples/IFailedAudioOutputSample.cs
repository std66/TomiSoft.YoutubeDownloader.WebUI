using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks;

namespace TomiSoft.YoutubeDownloader.Tests.Samples.FailedAudioDownloadSamples {
    interface IFailedAudioOutputSample : IAudioDownloadOutputSample {
        IEnumerable<KeyValuePair<SimpleTextConsoleAppMock.WriteTo, string>> Output { get; }
        int ExitCode { get; }
    }
}
