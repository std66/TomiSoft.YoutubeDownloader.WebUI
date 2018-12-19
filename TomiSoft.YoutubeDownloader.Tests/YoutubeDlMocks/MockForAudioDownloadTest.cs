using System.Collections.Generic;
using System.Linq;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks {
    class MockForAudioDownloadTest : BaseAudioDownloadMock {
        public MockForAudioDownloadTest(IEnumerable<string> StdOut, int ExitCode) 
            : base(StdOut.Select(x => To.Out(x)), ExitCode, Behavior.RunMainOnStart) {

        }

        public MockForAudioDownloadTest(IEnumerable<KeyValuePair<WriteTo, string>> Output, int ExitCode)
            : base(Output, ExitCode, Behavior.RunMainOnStart) {

        }
    }
}
