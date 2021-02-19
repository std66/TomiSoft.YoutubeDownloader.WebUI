using System;
using TomiSoft.Common.FileManagement;
using TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YouTubeDownloader.BusinessLogic.BusinessModels;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services
{
    public interface IDownloadManagementService {
        int ActiveDownloadCount { get; }
        DownloadStatusBM GetDownloadStatus(Guid downloadID);
        void StartDownload(DownloadRequestBM request);
        IFile GetDownloadedFile(Guid downloadId);
    }
}