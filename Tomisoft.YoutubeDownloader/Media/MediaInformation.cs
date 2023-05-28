using Newtonsoft.Json;

namespace TomiSoft.YoutubeDownloader.Media {
	public class MediaInformation : IMediaInformation {
		[JsonProperty("extractor")]
		internal string Extractor { get; set; }

		[JsonProperty("webpage_url")]
		public string MediaUri { get; set; }

		[JsonProperty("duration")]
		public double Duration { get; set; }

		[JsonProperty("id")]
		public string VideoId { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("thumbnail")]
		public string Thumbnail { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("is_live", NullValueHandling = NullValueHandling.Ignore)]
		public bool IsLiveStream { get; set; }
	}
}