using System;
using System.Collections.Generic;
using System.Text;

namespace Tomisoft.YoutubeDownloader.Media {
    public interface IMediaInformation {
        string MediaUri { get; }
        string Title { get; }
        double Duration { get; }
    }
}
