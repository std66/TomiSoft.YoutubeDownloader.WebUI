using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.WebUI.Core.Media {
	public interface ITagInfoWriter {
		bool Write(string path, IIdentifiedSong songMetadata);
	}
}