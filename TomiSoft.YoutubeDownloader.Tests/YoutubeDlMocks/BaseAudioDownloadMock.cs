using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks {
    abstract class BaseAudioDownloadMock : SimpleTextConsoleAppMock {
        protected override bool CheckCmdLineArgs(IEnumerable<string> args) {
            string[] arr = args.ToArray();
            return arr[0] == "--newline" && arr[1] == "--extract-audio" && arr[2] == "--audio-format" && arr[3] == "mp3" && arr[4] == "-o" && this.IsValidOutputFilename(arr[5]) && Uri.IsWellFormedUriString(arr[6], UriKind.Absolute);
        }

        public BaseAudioDownloadMock(IEnumerable<KeyValuePair<WriteTo, string>> Data, int ReturnedExitCode, Behavior behavior) 
            :base (Data, ReturnedExitCode, behavior) {

        }

        private bool IsValidOutputFilename(string Filename) {
            string TargetPath = Path.GetTempPath();

            string[] parts = Filename.Substring(TargetPath.Length).Split('.');
            return Guid.TryParse(parts[0], out _) && parts[1] == "%(ext)s";
        }
    }
}
