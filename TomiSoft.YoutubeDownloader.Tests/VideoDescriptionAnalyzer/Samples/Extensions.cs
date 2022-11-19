using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer.Samples {
	internal static class Extensions {
		public static IMediaInformation CreateMediaInfoMock(this IChapterExtractTestSample sample) {
			Mock<IMediaInformation> mock = new Mock<IMediaInformation>();

			mock
				.SetupGet(x => x.Description)
				.Returns(sample.Description);

			mock
				.SetupGet(x => x.Duration)
				.Returns(sample.Duration);

			return mock.Object;
		}

		public static void RunTest(this IChapterExtractTestSample sample, IChapterFinder finder) {
			//arrange
			IMediaInformation mediaInfo = sample.CreateMediaInfoMock();

			//act
			bool actualResult = finder.TryFindChapters(mediaInfo, out IReadOnlyList<Chapter> actualChapters);

			//assert
			if (sample.ShouldSucceed) {
				Assert.IsTrue(actualResult, $"{nameof(ChapterFinder)}.{nameof(ChapterFinder.TryFindChapters)} has returned False, meaning that the extractor did not find any chapters in the description text.");
				Assert.IsNotNull(actualChapters, $"{nameof(ChapterFinder)}.{nameof(ChapterFinder.TryFindChapters)} has returned a NULL-collection.");
				CollectionAssert.AreEquivalent(sample.ExpectedOutput.ToArray(), actualChapters.ToArray(), $"{nameof(ChapterFinder)}.{nameof(ChapterFinder.TryFindChapters)} has not returned the expected chapters.");
			}
			else {
				Assert.IsFalse(actualResult, $"{nameof(ChapterFinder)}.{nameof(ChapterFinder.TryFindChapters)} has returned True, meaning that the extractor has found chapters in the description text, but it shoudn't have.");
				Assert.IsNotNull(actualChapters, $"{nameof(ChapterFinder)}.{nameof(ChapterFinder.TryFindChapters)} has returned a NULL-collection.");
				Assert.IsTrue(actualChapters.Count == 0, $"{nameof(ChapterFinder)}.{nameof(ChapterFinder.TryFindChapters)} did not return an empty collection of chapters.");
			}
		}
	}
}
