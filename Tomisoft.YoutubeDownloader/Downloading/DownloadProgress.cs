using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader.Downloading {
    public class DownloadProgress : IDownload {
        private DownloadState downloadState = DownloadState.WaitingForStart;
        private readonly ProcessOutputDecorator DownloaderProcess;
        private double percentage;
        private bool Disposed = false;
        private readonly Guid FilenameGuid;
        private readonly string TargetPath;

        private const string DownloadStatusRegex = @"^\[download\]\s+(?<percent>\d+\.\d+)%";
        private const string PostProcessStatusRegex = @"^\[ffmpeg\]";

        public DownloadState Status {
            get {
                return this.downloadState;
            }
            internal set {
                bool changed = this.downloadState != value;

                this.downloadState = value;

                if (changed)
                    this.DownloadStatusChanged?.Invoke(this, value);
            }
        }

        public double Percentage {
            get {
                return this.percentage;
            }

            private set {
                this.percentage = value;
                this.PercentageChanged?.Invoke(this, value);
            }
        }

        public string Filename {
            get {
                if (this.downloadState != DownloadState.Completed)
                    throw new InvalidOperationException("Filename can only be retrieved in Completed state.");

                return Directory.GetFiles(this.TargetPath, $"{this.FilenameGuid}.*").First();
            }
        }

        public string ErrorMessage => this.DownloaderProcess.GetErrorAsString();
        public string CommandLine => this.DownloaderProcess.CommandLine;

        public event EventHandler<DownloadState> DownloadStatusChanged;
        public event EventHandler<double> PercentageChanged;

        internal DownloadProgress(IProcess DownloaderProcess, Guid FilenameGuid, string TargetDirectory) {
            this.DownloaderProcess = new ProcessOutputDecorator(DownloaderProcess);
            this.DownloaderProcess.ErrorDataReceived += this.ErrorDataReceived;
            this.DownloaderProcess.Exited += this.Exited;

            this.DownloaderProcess.RegisterStdOutLineProcessor(DownloadStatusRegex, PercentStatusReceived);
            this.DownloaderProcess.RegisterStdOutLineProcessor(PostProcessStatusRegex, PostProcessStatusReceived);
            
            this.FilenameGuid = FilenameGuid;
            this.TargetPath = TargetDirectory;
        }
        
        public void Dispose() {
            if (!Disposed) {
                this.DownloaderProcess.Exited -= this.Exited;
                this.DownloaderProcess.ErrorDataReceived -= this.ErrorDataReceived;

                this.Disposed = true;
            }

            this.DownloaderProcess.Dispose();
        }

        public void Start() {
            if (Status == DownloadState.WaitingForStart) {
                DownloaderProcess.Start();
                this.Status = DownloadState.Starting;
            }
        }

        private void PercentStatusReceived(IReadOnlyDictionary<string, string> args) {
            this.Status = DownloadState.Downloading;
            this.Percentage = Convert.ToDouble(args["percent"], CultureInfo.InvariantCulture);
        }

        private void PostProcessStatusReceived(IReadOnlyDictionary<string, string> args) {
            this.Status = DownloadState.PostProcessing;
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            Trace.WriteLine(e.Data);
        }

        private void Exited(object sender, EventArgs e) {
            this.Status = this.DownloaderProcess.ExitCode == 0 ? DownloadState.Completed : DownloadState.Failed;
        }
    }
}
