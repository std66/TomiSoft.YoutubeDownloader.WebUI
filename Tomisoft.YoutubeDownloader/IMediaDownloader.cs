using System;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YoutubeDownloader {
	public interface IMediaDownloader {
		IMediaInformation GetMediaInformation(Uri MediaUri);
		string GetVersion();
		IDownload PrepareDownload(Uri MediaUri, MediaFormat MediaFormat, string downloadDirectory);
		void Update();
	}
}