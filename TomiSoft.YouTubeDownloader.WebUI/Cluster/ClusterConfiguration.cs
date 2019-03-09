using System;
using System.Collections.Generic;

namespace TomiSoft.YouTubeDownloader.WebUI.Cluster {
    public class ClusterConfiguration : IClusterConfiguration {
        public IEnumerable<Uri> Hosts { get; set; }
    }
}
