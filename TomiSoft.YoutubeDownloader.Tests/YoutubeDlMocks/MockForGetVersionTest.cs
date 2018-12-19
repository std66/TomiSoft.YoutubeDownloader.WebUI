using System.Collections.Generic;
using System.Linq;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks {
    class MockForGetVersionTest : BaseProcessMock {
        private readonly string SampleOutput;
        
        public MockForGetVersionTest(string SampleOutput) {
            this.SampleOutput = SampleOutput;
        }
        
        protected override bool CheckCmdLineArgs(IEnumerable<string> args) {
            return args != null && args.Count() == 1 && args.First() == "--version";
        }

        protected override int Main(string[] args) {
            Write(this.SampleOutput);
            return 0;
        }
    }
}
