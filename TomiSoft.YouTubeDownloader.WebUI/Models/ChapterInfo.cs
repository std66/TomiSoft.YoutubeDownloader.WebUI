using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YouTubeDownloader.WebUI.Models {
	public record class ChapterInfo(string StartTime, string EndTime, double Duration, string Title) {
		public static ChapterInfo CreateFrom(Chapter chapter) {
			return new ChapterInfo(
				StartTime: chapter.StartTime.ToString(),
				EndTime: chapter.EndTime.ToString(),
				Duration: (chapter.EndTime - chapter.StartTime).TotalSeconds,
				Title: chapter.Title
			);
		}
	}
}
