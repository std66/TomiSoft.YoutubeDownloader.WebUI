using System.Collections.Generic;
using System.Linq;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YouTubeDownloader.WebUI.Models {
	public record class GetMediaInformationResponse(string MediaUri, string Title, double Duration, string VideoId, string Thumbnail, string Description, IReadOnlyList<ChapterInfo> Chapters) {
		public static GetMediaInformationResponse CreateFrom(IMediaInformation mediaInformation) {
			bool chaptersDetected = mediaInformation.TryFindChapters(out IReadOnlyList<Chapter> chapters);

			return new GetMediaInformationResponse(
				MediaUri: mediaInformation.MediaUri,
				Title: mediaInformation.Title,
				Duration: mediaInformation.Duration,
				VideoId: mediaInformation.VideoId,
				Thumbnail: mediaInformation.Thumbnail,
				Description: mediaInformation.Description,
				Chapters: chaptersDetected ? chapters.Select(x => ChapterInfo.CreateFrom(x)).ToList() : null
			);
		}
	}
}