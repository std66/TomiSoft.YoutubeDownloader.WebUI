using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.Tests.Samples.SuccessfulAudioDownloadSamples {
	class SuccessfulSoundcloudAudioOutputSample : ISuccessfulAudioOutputSample {
		public IEnumerable<DownloadState> ExpectedDownloadStatuses => new DownloadState[] {
		DownloadState.Starting,
		DownloadState.Downloading,
		DownloadState.PostProcessing,
		DownloadState.Completed
	};

		public IEnumerable<double> ExpectedPercents => new double[] {
		0.1,
		0.2,
		0.5,
		1.1,
		2.3,
		4.7,
		9.5,
		19.1,
		38.3,
		76.6,
		100.0
	};

		public string MediaUri => "https://soundcloud.com/yamato-official/yamato-shinwa";

		public IEnumerable<string> GetStdOut() => new List<string>() {
		"[soundcloud] yamato-official/yamato-shinwa: Resolving id",
		"[soundcloud] yamato-official/yamato-shinwa: Downloading info JSON",
		"[soundcloud] 297714439: Downloading track url",
		"[soundcloud] 297714439: Downloading m3u8 information",
		"[soundcloud] 297714439: Downloading m3u8 information",
		"[soundcloud] 297714439: Checking http_mp3_128_url video format URL",
		"[soundcloud] 297714439: Checking hls_mp3_128_url video format URL",
		"[soundcloud] 297714439: Checking hls_opus_64_url video format URL",
		"[download] Destination: Yamato - Shinwa (Original Mix)-297714439.mp3",
		"[download]   0.1% of 1.30MiB at 333.20KiB/s ETA 00:04",
		"[download]   0.2% of 1.30MiB at 999.60KiB/s ETA 00:01",
		"[download]   0.5% of 1.30MiB at  1.71MiB/s ETA 00:00",
		"[download]   1.1% of 1.30MiB at  3.66MiB/s ETA 00:00",
		"[download]   2.3% of 1.30MiB at  6.06MiB/s ETA 00:00",
		"[download]   4.7% of 1.30MiB at  1.58MiB/s ETA 00:00",
		"[download]   9.5% of 1.30MiB at  1.70MiB/s ETA 00:00",
		"[download]  19.1% of 1.30MiB at  1.82MiB/s ETA 00:00",
		"[download]  38.3% of 1.30MiB at  2.17MiB/s ETA 00:00",
		"[download]  76.6% of 1.30MiB at  2.40MiB/s ETA 00:00",
		"[download] 100.0% of 1.30MiB at  2.49MiB/s ETA 00:00",
		"[download] 100% of 1.30MiB in 00:00",
		"[ffmpeg] Post-process file Yamato - Shinwa (Original Mix)-297714439.mp3 exists, skipping",
		""
	};
	}
}