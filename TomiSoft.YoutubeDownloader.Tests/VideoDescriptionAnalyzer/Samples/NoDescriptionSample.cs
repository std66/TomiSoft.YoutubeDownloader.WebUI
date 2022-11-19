using System;
using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer.Samples {
	internal class NoDescriptionSample : IChapterExtractTestSample {
		public string Description => null;
		public double Duration => 1.0;
		public bool ShouldSucceed => false;
		public IReadOnlyList<Chapter> ExpectedOutput => Array.Empty<Chapter>();
	}
}
