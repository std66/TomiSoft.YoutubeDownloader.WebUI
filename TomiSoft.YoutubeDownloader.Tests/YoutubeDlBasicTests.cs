using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.Exceptions;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks;

namespace TomiSoft.YoutubeDownloader.Tests {
	[TestClass]
	public class YoutubeDlBasicTests {
		[TestMethod]
		public async Task CanGetVersionAsync() {
			string expectedVersion = "2018.12.11.";

			MockForGetVersionTest mock = new(expectedVersion);

			YoutubeDl downloader = new(mock);
			string actual = await downloader.GetVersionAsync();

			Assert.IsTrue(mock.ParametersPassedCorrectly);
			Assert.IsTrue(mock.ProcessStarted);
			Assert.AreEqual(expectedVersion, actual);
		}

		[TestMethod]
		public async Task CanHandleGetMediaInformationExceptionalCasesAsync() {
			MockForGetMediaInformationFaultyTest mock = new();
			YoutubeDl dl = new(mock);

			await Assert.ThrowsAsync<MediaInformationExtractException>(async () => {
				await dl.GetMediaInformationAsync(new Uri("http://something.com"));
			});
		}

		[TestMethod]
		public async Task CanGetYoutubeMediaInformationAsync() {
			string ExpectedVideoTitle = "sample-title";
			string ExpectedVideoID = "hW9tIK5pcl8";
			string ExpectedUploaderID = "sample-uploader";
			string ExpectedDescription = "sample-description";
			int ExpectedLikes = 51;
			int ExpectedDislikes = 12;
			int ExpectedViews = 314;
			double ExpectedDuration = 312.54;
			string ExpectedVideoUri = $"https://www.youtube.com/watch?v={ExpectedVideoID}";
			string ExpectedIdentifiedArtist = "sample-artist";
			string ExpectedIdentifiedSong = "sample-song";
			string ExpectedUploadDate = "20181216";

			MockForGetMediaInformationWithYoutubeTest processMock = new(
				ExpectedVideoTitle,
				ExpectedVideoID,
				ExpectedUploaderID,
				ExpectedDescription,
				ExpectedLikes,
				ExpectedDislikes,
				ExpectedViews,
				ExpectedDuration,
				ExpectedVideoUri,
				ExpectedIdentifiedArtist,
				ExpectedIdentifiedSong,
				ExpectedUploadDate
			);

			YoutubeDl downloader = new(processMock);
			IMediaInformation actual = await downloader.GetMediaInformationAsync(new Uri(ExpectedVideoUri));

			Assert.IsTrue(processMock.ProcessStarted, "Youtube-dl was not started.");
			Assert.IsTrue(processMock.ParametersPassedCorrectly, "Command-line arguments were passed incorrectly to youtube-dl.");

			Assert.IsNotNull(actual);
			Assert.IsInstanceOfType<ILikeableMedia>(actual);
			Assert.IsInstanceOfType<IIdentifiedSong>(actual);

			Assert.AreEqual(ExpectedVideoUri, actual.MediaUri);
			Assert.AreEqual(ExpectedDuration, actual.Duration);
			Assert.AreEqual(ExpectedVideoTitle, actual.Title);
			Assert.AreEqual(ExpectedDescription, actual.Description);
			Assert.AreEqual(ExpectedVideoID, actual.VideoId);

			ILikeableMedia actual2 = (ILikeableMedia)actual;
			Assert.AreEqual(ExpectedLikes, actual2.Likes);
			Assert.AreEqual(ExpectedDislikes, actual2.Dislikes);

			IIdentifiedSong actual3 = (IIdentifiedSong)actual;
			Assert.AreEqual(ExpectedIdentifiedArtist, actual3.Artist);
			Assert.AreEqual(ExpectedIdentifiedSong, actual3.TrackTitle);
		}
	}
}