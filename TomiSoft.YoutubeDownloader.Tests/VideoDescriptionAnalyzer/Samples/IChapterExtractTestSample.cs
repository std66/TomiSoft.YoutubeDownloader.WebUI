using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer.Samples {
	internal interface IChapterExtractTestSample {
		string Description { get; }
		double Duration { get; }
		IReadOnlyList<Chapter> ExpectedOutput { get; }
		bool ShouldSucceed { get; }
	}
}