using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.WebUI.Models {
	public record class GetMediaInformationResponse(string MediaUri, string Title, double Duration, string VideoId, string Thumbnail, string Description) {
		public static GetMediaInformationResponse CreateFrom(IMediaInformation mediaInformation) {
			return new GetMediaInformationResponse(
				MediaUri: mediaInformation.MediaUri,
				Title: mediaInformation.Title,
				Duration: mediaInformation.Duration,
				VideoId: mediaInformation.VideoId,
				Thumbnail: mediaInformation.Thumbnail,
				Description: mediaInformation.Description
			);
		}
	}
}