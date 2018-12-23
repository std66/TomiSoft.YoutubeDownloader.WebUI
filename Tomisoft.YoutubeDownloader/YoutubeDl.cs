using Newtonsoft.Json;
using System;
using System.IO;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Exceptions;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YoutubeDownloader {
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
                    string Extractor = JsonConvert.DeserializeObject<MediaInformation>(StdOut).Extractor;

                    switch (Extractor) {
                        case "youtube":
                            return JsonConvert.DeserializeObject<YoutubeMediaInformation>(StdOut);

                        default:
                            return JsonConvert.DeserializeObject<MediaInformation>(StdOut);
                    }
                }

                throw new MediaInformationExtractException(p, MediaUri.ToString());
            }
        }

        public IDownload PrepareDownload(Uri MediaUri, MediaFormat MediaFormat) {
            IProcess process = null;

            Guid filenameGuid = Guid.NewGuid();
            string targetDirectory = Path.GetTempPath();
            string TargetFilename = Path.Combine(targetDirectory, $"{filenameGuid}.%(ext)s");

            switch (MediaFormat) {
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
    }
}
