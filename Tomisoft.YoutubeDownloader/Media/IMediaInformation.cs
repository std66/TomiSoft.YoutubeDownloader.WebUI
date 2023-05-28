namespace TomiSoft.YoutubeDownloader.Media {
	public interface IMediaInformation {
		string MediaUri { get; }
		string Title { get; }
		double Duration { get; }
		string VideoId { get; }
		string Thumbnail { get; }
		string Description { get; }
		bool IsLiveStream { get; }
	}
}