using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer {
	public interface IChapterFinder {
		bool TryFindChapters(IMediaInformation mediaInfo, out IReadOnlyList<Chapter> chapters);
	}
}