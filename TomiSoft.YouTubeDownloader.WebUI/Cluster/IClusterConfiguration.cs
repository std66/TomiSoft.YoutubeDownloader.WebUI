using System;
using System.Collections.Generic;

namespace TomiSoft.YouTubeDownloader.WebUI.Cluster {
    interface IClusterConfiguration {
        IEnumerable<Uri> Hosts { get; }
    }
}
