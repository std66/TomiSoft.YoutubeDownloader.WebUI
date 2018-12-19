using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tomisoft.YoutubeDownloader {
    internal class CapturingProcess : Process, IProcess {
        private readonly string ExecutablePath;
        private readonly IEnumerable<string> Arguments;
        private readonly List<string> output = new List<string>();
        private readonly List<string> error = new List<string>();

        public IReadOnlyList<string> StandardOutputLines => this.output;
        public IReadOnlyList<string> StandardErrorLines => this.error;
        public bool ExitedSuccessfully => this.HasExited && this.ExitCode == 0;

        public CapturingProcess(string ExecutablePath, params string[] Arguments)
            : this(ExecutablePath, (IEnumerable<string>)Arguments) {
        }

        public CapturingProcess(string ExecutablePath, IEnumerable<string> Arguments) {
            this.ExecutablePath = ExecutablePath;
            this.Arguments = new List<string>(Arguments ?? Array.Empty<string>());

            this.StartInfo = this.CreateProcess();
            this.OutputDataReceived += CapturingProcess_OutputDataReceived;
            this.ErrorDataReceived += CapturingProcess_ErrorDataReceived;
            this.EnableRaisingEvents = true;
        }
        
        public new void Start() {
            this.output.Clear();
            this.error.Clear();

            base.Start();
            base.BeginOutputReadLine();
            base.BeginErrorReadLine();
        }

        public string GetOutputAsString() {
            return String.Join(String.Empty, this.output);
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            
            this.OutputDataReceived -= this.CapturingProcess_OutputDataReceived;
            this.ErrorDataReceived -= this.CapturingProcess_ErrorDataReceived;
        }

        private void CapturingProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            this.error.Add(e.Data);
        }

        private void CapturingProcess_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            this.output.Add(e.Data);
        }

        private ProcessStartInfo CreateProcess() {
            return new ProcessStartInfo {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = this.ExecutablePath,
                Arguments = String.Join(' ', this.Arguments)
            };
        }
    }
}
