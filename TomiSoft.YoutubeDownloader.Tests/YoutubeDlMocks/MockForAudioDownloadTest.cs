using System.Collections.Generic;
using System.Linq;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks
{
    class MockForAudioDownloadTest : BaseAudioDownloadMock {
        public MockForAudioDownloadTest(IEnumerable<string> StdOut, int ExitCode, string DownloadPath) 
            : base(StdOut.Select(x => To.Out(x)), ExitCode, Behavior.RunMainOnStart, DownloadPath) {

        }

        public MockForAudioDownloadTest(IEnumerable<KeyValuePair<WriteTo, string>> Output, int ExitCode, string DownloadPath)
            : base(Output, ExitCode, Behavior.RunMainOnStart, DownloadPath) {

        }
    }
}
