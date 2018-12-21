using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Tomisoft.YoutubeDownloader;
using Tomisoft.YoutubeDownloader.Downloading;
using Tomisoft.YoutubeDownloader.Media;
using TomiSoft.YoutubeDownloader.Tests.Samples;
using TomiSoft.YoutubeDownloader.Tests.Samples.FailedAudioDownloadSamples;
using TomiSoft.YoutubeDownloader.Tests.Samples.SuccessfulAudioDownloadSamples;
using TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks;

namespace TomiSoft.YoutubeDownloader.Tests {
    [TestClass]
    public class YoutubeDlAudioDownloadTests {
        [TestMethod]
        public void CanDownloadYoutubeAudio() {
            PerformTest(
                new SuccessfulYoutubeAudioOutputSample()
            );
        }

        [TestMethod]
        public void CanDownloadSoundcloudAudio() {
            PerformTest(
                new SuccessfulSoundcloudAudioOutputSample()
            );
        }

        [TestMethod]
        public void CanHandleCommonDownloadFailures() {
            PerformTest(
                new SimpleDownloadFailedSample()
            );
        }

        private void PerformTest(IAudioDownloadOutputSample sample) {
            MockForAudioDownloadTest processMock = this.CreateProcessFactory(sample);

            YoutubeDl downloader = new YoutubeDl(processMock);

            Dictionary<double, bool> ExpectedPercentsToBeReported = new Dictionary<double, bool>(sample.ExpectedPercents.Distinct().Select(x => new KeyValuePair<double, bool>(x, false)));
            Dictionary<DownloadState, bool> ExpectedStatusesToBeReported = new Dictionary<DownloadState, bool>(sample.ExpectedDownloadStatuses.Distinct().Select(x => new KeyValuePair<DownloadState, bool>(x, false)));

            using (DownloadProgress progress = downloader.PrepareDownload(new Uri(sample.MediaUri), MediaFormat.MP3Audio)) {
                progress.DownloadStatusChanged += (o, e) => {
                    Assert.AreEqual(e, progress.Status, $"The download status value reported by event {nameof(DownloadProgress.DownloadStatusChanged)} ({e}) is not equal to the value set in {nameof(DownloadProgress.Status)} ({progress.Status}).");
                    ExpectedStatusesToBeReported[e] = true;
                };

                progress.PercentageChanged += (o, e) => {
                    Assert.AreEqual(e, progress.Percentage, $"The download percentage value reported by event {nameof(DownloadProgress.PercentageChanged)} ({e}) is not equal to the value set in {nameof(DownloadProgress.Percentage)} ({progress.Percentage}).");
                    Assert.AreEqual(DownloadState.Downloading, progress.Status, $"The download percentage is changed when {nameof(DownloadProgress.Status)} is not {DownloadState.Downloading}.");
                    Assert.IsTrue(ExpectedPercentsToBeReported.ContainsKey(e), $"Unexpected download percentage reported: {e}.");
                    ExpectedPercentsToBeReported[e] = true;
                };

                Assert.AreEqual(DownloadState.WaitingForStart, progress.Status, $"Download status was not in {DownloadState.WaitingForStart} status.");
                Assert.AreEqual(0.0, progress.Percentage, $"Download process percentage was not 0.0 when status is {DownloadState.WaitingForStart}.");
                Assert.IsFalse(processMock.ProcessStarted, $"Download process was started automatically, without invoking {nameof(DownloadProgress.Start)}.");

                progress.Start();
                Assert.IsTrue(processMock.ProcessStarted, $"Youtube-dl was not started after invoking {nameof(DownloadProgress.Start)}.");
                Assert.IsTrue(processMock.ParametersPassedCorrectly, "Command-line arguments were passed incorrectly to youtube-dl.");
            }

            foreach (var item in ExpectedStatusesToBeReported) {
                Assert.IsTrue(item.Value, $"Expected download status {item.Key} was not reported by event {nameof(DownloadProgress.DownloadStatusChanged)}.");
            }

            foreach (var item in ExpectedPercentsToBeReported) {
                Assert.IsTrue(item.Value, $"Expected download percentage {item.Key} was not reported by event {nameof(DownloadProgress.PercentageChanged)}.");
            }
        }

        private MockForAudioDownloadTest CreateProcessFactory(IAudioDownloadOutputSample sample) {
            if (sample is ISuccessfulAudioOutputSample successfulSample) {
                return new MockForAudioDownloadTest(successfulSample.GetStdOut(), 0);
            }
            else if (sample is IFailedAudioOutputSample failedSample) {
                return new MockForAudioDownloadTest(failedSample.Output, failedSample.ExitCode);
            }

            throw new System.NotSupportedException($"{sample.GetType().FullName} is not supported.");
        }
    }
}
