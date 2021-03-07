using System;

namespace TomiSoft.YouTubeDownloader.WebUI {
    public static class AppVersion {
        public static string GitCommitHash => "GIT_COMMIT_HASH";
        public static string GitShortCommitHash => "GIT_SHORT_COMMIT_HASH";
        public static string GitBranchName => "GIT_BRANCH";
        public static DateTimeOffset BuildTime { get; } = DateTimeOffset.Parse("BUILD_TIME");
        public static Version Version { get; } = new Version("VERSION");
    }
}