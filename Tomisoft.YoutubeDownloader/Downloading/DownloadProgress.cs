using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tomisoft.YoutubeDownloader.Downloading {
    public class DownloadProgress : IDisposable {
        private DownloadState downloadState = DownloadState.WaitingForStart;
        private readonly IProcess DownloaderProcess;
        private double percentage;
        private bool Disposed = false;
        private readonly Guid FilenameGuid;
        private readonly string TargetPath;

        private static readonly Regex DownloadPercentRegex = new Regex(@"^\[download\]\s+(?<percent>\d+\.\d+)%");

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

        public event EventHandler<DownloadState> DownloadStatusChanged;
        public event EventHandler<double> PercentageChanged;

        internal DownloadProgress(IProcess DownloaderProcess, Guid FilenameGuid, string TargetDirectory) {
            this.DownloaderProcess = DownloaderProcess;
            this.DownloaderProcess.OutputDataReceived += this.OutputDataReceived;
            this.DownloaderProcess.ErrorDataReceived += this.ErrorDataReceived;
            this.DownloaderProcess.Exited += this.Exited;
            
            this.FilenameGuid = FilenameGuid;
            this.TargetPath = TargetDirectory;
        }
        
        public void Dispose() {
            if (!Disposed) {
                this.DownloaderProcess.OutputDataReceived -= this.OutputDataReceived;
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

        private void OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (e.Data == null)
                return;

            Match m = DownloadPercentRegex.Match(e.Data);
            if (m.Success) {
                this.Status = DownloadState.Downloading;
                this.Percentage = Convert.ToDouble(m.Groups["percent"].Value, CultureInfo.InvariantCulture);
            }
            else if (e.Data.StartsWith("[ffmpeg]")) {
                this.Status = DownloadState.PostProcessing;
            }
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            Trace.WriteLine(e.Data);
        }

        private void Exited(object sender, EventArgs e) {
            this.Status = this.DownloaderProcess.ExitCode == 0 ? DownloadState.Completed : DownloadState.Failed;
        }
    }
}
