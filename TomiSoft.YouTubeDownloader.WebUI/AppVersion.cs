using System;

namespace TomiSoft.YouTubeDownloader.WebUI {
	public static class AppVersion {
		public static string GitCommitHash => "0000000000000000000000000000000000000000";
		public static string GitShortCommitHash => "0000000";
		public static string GitBranchName => "no_branch";
		public static DateTimeOffset BuildTime { get; } = DateTimeOffset.Parse("2021-03-06T23:32:51.4626957+00:00");
		public static Version Version { get; } = new Version("1.0.0");
	}
}