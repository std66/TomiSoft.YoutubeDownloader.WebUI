using System;
using TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YoutubeDownloader.BusinessLogic.Dependencies.IO;
using TomiSoft.YouTubeDownloader.BusinessLogic.BusinessModels;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
	public interface IDownloadManagementService {
		int ActiveDownloadCount { get; }
		bool TryGetDownloadStatus(Guid downloadID, out DownloadStatusBM downloadStatus);
		void StartDownload(DownloadRequestBM request);
		bool TryGetDownloadedFile(Guid downloadId, out IFile file);
	}
}