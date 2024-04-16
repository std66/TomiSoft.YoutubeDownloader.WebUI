using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TomiSoft.Common.SystemProcess;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Exceptions;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YoutubeDownloader {
	public class YoutubeDl : IMediaDownloader {
		private readonly IProcessFactory ProcessFactory;

		public YoutubeDl(IProcessFactory ProcessFactory) {
			this.ProcessFactory = ProcessFactory;
		}

		public async Task<string> GetVersionAsync() {
			var result = await this.ProcessFactory.CreateAsyncProcess().StartAsync("--version");

			if (!result.ExitedSuccessfully)
				throw new Exception("There was an error while getting YoutubeDl's version.");

			return result.StdOutLines[0];
		}

		public async Task<IMediaInformation> GetMediaInformationAsync(Uri MediaUri) {
			var result = await this.ProcessFactory.CreateAsyncProcess().StartAsync("--dump-json", MediaUri.ToString());

			if (!result.ExitedSuccessfully) {
				TryFindMediaInformationQueryFailureReason(result, MediaUri);
				throw new MediaInformationExtractException(result, MediaUri);
			}

			return MediaInformationFactory.Create(result.StdOut);
		}

		private void TryFindMediaInformationQueryFailureReason(ProcessExecutionResult p, Uri MediaUri) {
			if (p.StdErrLines.Any(x => x == "ERROR: Private video"))
				throw new PrivateMediaException(p, MediaUri);

			if (p.StdErrLines.Any(x => x.Contains("This video is only available to Music Premium members")))
				throw new RestrictedBySubscriptionPolicyException(p, MediaUri);
		}

		public IDownload PrepareDownload(Uri MediaUri, MediaFormat MediaFormat, string downloadDirectory) {
			Guid filenameGuid = Guid.NewGuid();
			string targetDirectory = downloadDirectory;
			string TargetFilename = Path.Combine(targetDirectory, $"{filenameGuid}.%(ext)s");

			IProcess process;
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

		public void Update() {
			using (IProcess p = this.ProcessFactory.Create("-U")) {
				p.Start();
				p.WaitForExit();
			}
		}
	}
}