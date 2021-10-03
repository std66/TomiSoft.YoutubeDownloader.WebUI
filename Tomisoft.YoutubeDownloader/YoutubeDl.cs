using System;
using System.IO;
using System.Linq;
using TomiSoft.Common.SystemProcess;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Exceptions;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YoutubeDownloader
{
    public class YoutubeDl : IMediaDownloader {
        private readonly IProcessFactory ProcessFactory;

        public YoutubeDl(IProcessFactory ProcessFactory) {
            this.ProcessFactory = ProcessFactory;
        }

        public string GetVersion() {
            using (IProcess p = this.ProcessFactory.Create("--version")) {
                p.Start();
                p.WaitForExit();

                if (p.ExitedSuccessfully)
                    return p.StandardOutputLines[0];

                throw new Exception("There was an error while getting YoutubeDl's version.");
            }
        }

        public IMediaInformation GetMediaInformation(Uri MediaUri) {
            using (IProcess p = this.ProcessFactory.Create("--dump-json", MediaUri.ToString())) {
                p.Start();
                p.WaitForExit();

                if (p.ExitedSuccessfully) {
                    string StdOut = p.GetOutputAsString();
                    return MediaInformationFactory.Create(StdOut);
                }

                TryFindMediaInformationQueryFailureReason(p, MediaUri);
                throw new MediaInformationExtractException(p, MediaUri);
            }
        }

        private void TryFindMediaInformationQueryFailureReason(IProcess p, Uri MediaUri)
        {
            if (p.StandardErrorLines.Any(x => x == "ERROR: Private video"))
                throw new PrivateMediaException(p, MediaUri);
        }

        public IDownload PrepareDownload(Uri MediaUri, MediaFormat MediaFormat, string downloadDirectory) {
            Guid filenameGuid = Guid.NewGuid();
            string targetDirectory = downloadDirectory;
            string TargetFilename = Path.Combine(targetDirectory, $"{filenameGuid}.%(ext)s");

            IProcess process;
            switch (MediaFormat)
            {
                case MediaFormat.MP3Audio:
                    process = this.ProcessFactory.Create("--newline", "--extract-audio", "--audio-format", "mp3", "-o", TargetFilename, MediaUri.ToString());
                    break;

                case MediaFormat.Video:
                    process = this.ProcessFactory.Create("--newline", "-o", TargetFilename, MediaUri.ToString());
                    break;

                default:
                    throw new NotSupportedException($"The requested media format ({MediaFormat}) is not supported.");
            }

            return new DownloadProgress(process, filenameGuid, targetDirectory);
        }

        public void Update() {
            using (IProcess p = this.ProcessFactory.Create("-U")) {
                p.Start();
                p.WaitForExit();
            }
        }
    }
}
