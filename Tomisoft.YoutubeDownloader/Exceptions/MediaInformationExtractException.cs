using System;

namespace TomiSoft.YoutubeDownloader.Exceptions {
    public class MediaInformationExtractException : Exception {
        public string MediaUri { get; }
        public string StandardError { get; }
        public int ExitCode { get; }

        public MediaInformationExtractException(IProcess Process, string MediaUri)
            : base($"There was an error while getting media information. Media URI: {MediaUri} YoutubeDl stderr: {Process.GetErrorAsString()}") {
            this.MediaUri = MediaUri;
            this.StandardError = Process.GetErrorAsString();
            this.ExitCode = Process.ExitCode;
        }
    }
}
