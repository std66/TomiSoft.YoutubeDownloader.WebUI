using System;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YoutubeDownloader {
	public interface IMediaDownloader {
		IMediaInformation GetMediaInformation(Uri MediaUri);
		Task<string> GetVersionAsync();
		IDownload PrepareDownload(Uri MediaUri, MediaFormat MediaFormat, string downloadDirectory);
		void Update();
	}
}