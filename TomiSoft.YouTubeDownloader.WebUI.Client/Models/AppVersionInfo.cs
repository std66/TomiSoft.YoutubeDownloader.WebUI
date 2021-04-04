using System;

namespace TomiSoft.YouTubeDownloader.WebUI.Client.Models {
    public class AppVersionInfo {
        public Version Version { get; set; }
        public string GitShortCommitHash { get; set; }
        public string GitCommitHash { get; set; }
        public DateTimeOffset BuildTime { get; set; }
        public string YoutubeDlVersion { get; set; }
    }
}
