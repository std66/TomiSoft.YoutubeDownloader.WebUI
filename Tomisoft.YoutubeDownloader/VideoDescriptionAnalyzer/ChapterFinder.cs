using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer {
	public class ChapterFinder : IChapterFinder {
		private static readonly Regex regex = new Regex(
			"^" +
			@"\W*(?<start>(\d{1,2}:)?\d{1,2}:\d{1,2})\W+" +
			@"(?<end>(\d{1,2}:)?\d{1,2}:\d{1,2})?(\W+)?" +
			@"(?<title>[^\r\n]+)" +
			"\\r?$",

			RegexOptions.Compiled | RegexOptions.Multiline
		);

		public bool TryFindChapters(IMediaInformation mediaInfo, out IReadOnlyList<Chapter> chapters) {
			chapters = Array.Empty<Chapter>();

			if (mediaInfo == null || mediaInfo.Description == null)
				return false;

			MatchCollection matches = regex.Matches(mediaInfo.Description);

			bool isMatch = matches.Count > 0;
			chapters = ExtractChapters(mediaInfo, matches);
			return isMatch && chapters.Count > 0;
		}

		private IReadOnlyList<Chapter> ExtractChapters(IMediaInformation mediaInfo, MatchCollection matches) {
			List<Chapter> result = new List<Chapter>();

			for (int i = 0; i < matches.Count; i++) {
				string startTimeStr = TimestampStringCorrector.Correct(matches[i].Groups["start"].Value);

				bool startTimeSuccess = TimeSpan.TryParse(startTimeStr, out TimeSpan startTime);
				bool endTimeSuccess = TryFindEndTime(mediaInfo, matches, i, out TimeSpan endTime);

				if (startTimeSuccess && endTimeSuccess && startTime < endTime) {
					result.Add(new Chapter(startTime, endTime, matches[i].Groups["title"].Value));
				}
			}

			return result;
		}

		private static bool TryFindEndTime(IMediaInformation mediaInfo, MatchCollection matches, int i, out TimeSpan endTime) {
			string endTimeStr = null;
			//if end time is provided
			if (matches[i].Groups["end"].Success) {
				endTimeStr = matches[i].Groups["end"].Value;
			}
			//if end time is not provided
			else {
				//if there is a next chapter, use it's start time as end
				if (i < matches.Count - 1)
					endTimeStr = matches[i + 1].Groups["start"].Value;
			}

			//if end time is still null, perhaps this is the last chapter in the video
			if (endTimeStr == null) {
				endTime = TimeSpan.FromSeconds(mediaInfo.Duration);
				return true;
			}
			else {
				endTimeStr = TimestampStringCorrector.Correct(endTimeStr);
				return TimeSpan.TryParse(endTimeStr, out endTime);
			}
		}
	}
}
