using System;
using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer.Samples {
	internal class TimestampsInFrontAndNoEndTimeForLastChapterSample : IChapterExtractTestSample {
		public string Description => @"So much love!

Tracklist:
00:00 - 01:45 / Intro
01:45 - 07:50 / Take It Smart (Original Mix)
07:50 - 13:35 / Thunderstorm (Original Mix)
13:35 - 18:22 / Space Diver (Original Mix)
18:22 - 26:40 / Gravity (Original Mix)
26:40 - 32:15 / The Troublemakerz (Original Mix)
32:15 - 39:26 / Nothing Seems To Be (Original Mix)
39:26 - 45:38 / Never Look Back (Original Mix)
45:38 - 51:45 / To The Moon And Back (Original Mix)
51:45 - 58:14 / Blue Lake (Original Mix)
58:14 - 1:04:25 / I Am The Joker (Original Mix)
1:04:25 - 1:10:09 / Game Over (Original Mix)
1:10:09 - 1:17:13 / Purple Noise (Original Mix)
1:17:13 - 1:23:28 / Hashtag (Original Mix)
1:23:28 - end / Angel In The Sky (Original Mix)

__
www.borisbrejcha.de
www.fckng-serious.de
www.tomorrowland.com
";

		public double Duration => new TimeSpan(01, 30, 14).TotalSeconds;
		public bool ShouldSucceed => true;
		public IReadOnlyList<Chapter> ExpectedOutput => new List<Chapter>() {
			new Chapter(new TimeSpan(00, 00, 00), new TimeSpan(00, 01, 45), "Intro"),
			new Chapter(new TimeSpan(00, 01, 45), new TimeSpan(00, 07, 50), "Take It Smart (Original Mix)"),
			new Chapter(new TimeSpan(00, 07, 50), new TimeSpan(00, 13, 35), "Thunderstorm (Original Mix)"),
			new Chapter(new TimeSpan(00, 13, 35), new TimeSpan(00, 18, 22), "Space Diver (Original Mix)"),
			new Chapter(new TimeSpan(00, 18, 22), new TimeSpan(00, 26, 40), "Gravity (Original Mix)"),
			new Chapter(new TimeSpan(00, 26, 40), new TimeSpan(00, 32, 15), "The Troublemakerz (Original Mix)"),
			new Chapter(new TimeSpan(00, 32, 15), new TimeSpan(00, 39, 26), "Nothing Seems To Be (Original Mix)"),
			new Chapter(new TimeSpan(00, 39, 26), new TimeSpan(00, 45, 38), "Never Look Back (Original Mix)"),
			new Chapter(new TimeSpan(00, 45, 38), new TimeSpan(00, 51, 45), "To The Moon And Back (Original Mix)"),
			new Chapter(new TimeSpan(00, 51, 45), new TimeSpan(00, 58, 14), "Blue Lake (Original Mix)"),
			new Chapter(new TimeSpan(00, 58, 14), new TimeSpan(01, 04, 25), "I Am The Joker (Original Mix)"),
			new Chapter(new TimeSpan(01, 04, 25), new TimeSpan(01, 10, 09), "Game Over (Original Mix)"),
			new Chapter(new TimeSpan(01, 10, 09), new TimeSpan(01, 17, 13), "Purple Noise (Original Mix)"),
			new Chapter(new TimeSpan(01, 17, 13), new TimeSpan(01, 23, 28), "Hashtag (Original Mix)"),
			new Chapter(new TimeSpan(01, 23, 28), new TimeSpan(01, 30, 14), "end / Angel In The Sky (Original Mix)")
		};
	}
}
