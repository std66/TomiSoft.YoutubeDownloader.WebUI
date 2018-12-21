using System;

namespace TomiSoft.YouTubeDownloader.WebUI.Data {
    public interface IFilenameDatabase : IDisposable {
        string GetFilename(Guid DownloadId);
        bool AddFilename(Uri MediaUri, string Filename);
        bool AssignDownloadId(Uri MediaUri, Guid DownloadId);
        void DeleteFilename(Guid DownloadId);
    }
}
