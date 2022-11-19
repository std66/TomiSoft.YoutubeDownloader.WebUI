using Microsoft.VisualStudio.TestTools.UnitTesting;
using TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer.Samples;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer {
	[TestClass]
	public class ChapterFinderTests {
		private readonly ChapterFinder finder = new ChapterFinder();

		[TestMethod]
		[Description("Tests that the chapter finder returns false when the media info parameter is null.")]
		public void ChapterFinder_ReturnsFalseWhenMediaInfoIsNull() {
			//arrange

			//act
			bool actual = finder.TryFindChapters(null, out _);

			//assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		[Description("Tests that the chapter finder returns false when the media's description string is null.")]
		public void ChapterFinder_ReturnsFalseWhenDescriptionIsNull() {
			new NoDescriptionSample().RunTest(finder);
		}

		[TestMethod]
		[Description("Tests that the chapter finder returns false when the description does not have chapter information.")]
		public void ChapterFinder_ReturnsFalseWhenDescriptionHasNoChapters() {
			new NoChaptersSample().RunTest(finder);
		}

		[TestMethod]
		[Description("Tests that the chapter finder sets the length of video as the end time of the last chapter when it is not given in the description.")]
		public void ChapterFinder_LastChapterHasNoEndTimeGiven() {
			new TimestampsInFrontAndNoEndTimeForLastChapterSample().RunTest(finder);
		}

		[TestMethod]
		[Description("Tests that the chapter finder considers the start time of the next chapter when no end times are given.")]
		public void ChapterFinder_OnlyStartTimesAreGiven() {
			new TimestampsInFrontAndOnlyStartTimesAreGivenSample().RunTest(finder);
		}

		[TestMethod]
		[Description("Tests that the chapter finder can recognize timestamps that are wrapped with special characters.")]
		public void ChapterFinder_TimestampIsWrappedWithSpecialCharacters() {
			new TimestampIsWrappedWithSquareParenthesesSample().RunTest(finder);
		}
	}
}
