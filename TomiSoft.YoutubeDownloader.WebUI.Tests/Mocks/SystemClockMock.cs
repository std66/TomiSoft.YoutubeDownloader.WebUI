using System;
using TomiSoft.YouTubeDownloader.WebUI.Core;

namespace TomiSoft.YoutubeDownloader.WebUI.Tests.Mocks {
    class SystemClockMock : ISystemClock {
        public DateTime UtcNow {
            get;
            set;
        }
    }
}
