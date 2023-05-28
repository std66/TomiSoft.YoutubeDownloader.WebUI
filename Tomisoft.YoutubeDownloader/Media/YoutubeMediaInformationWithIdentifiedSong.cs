using Newtonsoft.Json;

namespace TomiSoft.YoutubeDownloader.Media {
	public class YoutubeMediaInformationWithIdentifiedSong : YoutubeMediaInformation, IIdentifiedSong {
		[JsonProperty("track")]
		public string TrackTitle { get; set; }

		[JsonProperty("artist")]
		public string Artist { get; set; }
	}
}