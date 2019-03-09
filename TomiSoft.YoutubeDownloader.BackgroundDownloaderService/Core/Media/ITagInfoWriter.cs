using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core.Media {
    public interface ITagInfoWriter {
        bool Write(string path, IIdentifiedSong songMetadata);
    }
}
