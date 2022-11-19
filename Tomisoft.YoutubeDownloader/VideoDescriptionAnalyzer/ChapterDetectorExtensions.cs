using System;
using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Media {
	public static class ChapterDetectorExtensions {
		public static bool TryFindChapters(this IMediaInformation mediaInfo, out IReadOnlyList<Chapter> chapters) {
			IChapterFinder[] detectors = new[] {
				new ChapterFinder()
			};

			foreach (var detector in detectors) {
				if (detector.TryFindChapters(mediaInfo, out chapters))
					return true;
			}

			chapters = Array.Empty<Chapter>();
			return false;
		}
	}
}
