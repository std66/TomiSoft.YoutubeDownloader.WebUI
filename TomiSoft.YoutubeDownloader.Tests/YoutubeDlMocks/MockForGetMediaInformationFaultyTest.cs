using System;
using System.Collections.Generic;
using System.Linq;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks {
    internal class MockForGetMediaInformationFaultyTest : SimpleTextConsoleAppMock {
        public MockForGetMediaInformationFaultyTest() 
            : base(GetSampleOutput(), 1, Behavior.RunMainOnWaitForExit) {
        }

        protected override bool CheckCmdLineArgs(IEnumerable<string> args) {
            return args.First() == "--dump-json" && Uri.IsWellFormedUriString(args.Last(), UriKind.Absolute);
        }

        private static IEnumerable<KeyValuePair<WriteTo, string>> GetSampleOutput() {
            return new KeyValuePair<WriteTo, string>[] {
                To.Err("Some error happened. Trace:"),
                To.Err("  unknown.c line 25")
            };
        }
    }
}
