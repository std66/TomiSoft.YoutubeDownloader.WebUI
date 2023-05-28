using Newtonsoft.Json;

namespace TomiSoft.YoutubeDownloader.Media {
	public class YoutubeMediaInformation : MediaInformation, ILikeableMedia {

		[JsonProperty("tags")]
		public string[] Tags { get; set; }

		[JsonProperty("like_count", NullValueHandling = NullValueHandling.Ignore)]
		public int Likes { get; set; }

		[JsonProperty("dislike_count", NullValueHandling = NullValueHandling.Ignore)]
		public int Dislikes { get; set; }

	}
}