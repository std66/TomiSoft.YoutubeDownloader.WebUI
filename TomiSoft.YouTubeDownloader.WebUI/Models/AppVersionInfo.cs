using System;

namespace TomiSoft.YouTubeDownloader.WebUI.Models {
	public class AppVersionInfo {
		public AppVersionInfo(string youtubeDlVersion) {
			Version = AppVersion.Version;
			GitShortCommitHash = AppVersion.GitShortCommitHash;
			GitCommitHash = AppVersion.GitCommitHash;
			BuildTime = AppVersion.BuildTime;
			YoutubeDlVersion = youtubeDlVersion;
		}

		public Version Version { get; }
		public string GitShortCommitHash { get; }
		public string GitCommitHash { get; }
		public DateTimeOffset BuildTime { get; }
		public string YoutubeDlVersion { get; }
	}
}