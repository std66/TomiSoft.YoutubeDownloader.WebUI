namespace TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer {
	internal class TimestampStringCorrector {
		public static string Correct(string timestamp) {
			var parts = timestamp.Split(':');

			if (parts.Length == 2)
				return $"00:{parts[0]}:{parts[1]}";

			return timestamp;
		}
	}
}
