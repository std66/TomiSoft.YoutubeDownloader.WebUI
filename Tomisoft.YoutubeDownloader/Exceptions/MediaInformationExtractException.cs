using System;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader.Exceptions {
    public class MediaInformationExtractException : Exception {
        public Uri MediaUri { get; }
        public string StandardError { get; }
        public int ExitCode { get; }

        public MediaInformationExtractException(IProcess Process, Uri MediaUri)
            : base($"There was an error while getting media information. Media URI: {MediaUri} YoutubeDl stderr: {Process.GetErrorAsString()}") {
            this.MediaUri = MediaUri;
            this.StandardError = Process.GetErrorAsString();
            this.ExitCode = Process.ExitCode;
        }
    }
}
