using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TomiSoft.Common.SystemProcess {
    public class CapturingProcess : ProcessFacade, IProcess {
        private readonly List<string> output = new List<string>();
        private readonly List<string> error = new List<string>();

        public override IReadOnlyList<string> StandardOutputLines => this.output;
        public override IReadOnlyList<string> StandardErrorLines => this.error;

        public CapturingProcess(string ExecutablePath, params string[] Arguments)
            : this(ExecutablePath, (IEnumerable<string>)Arguments) {
        }

        public CapturingProcess(string ExecutablePath, IEnumerable<string> Arguments)
            : base(ExecutablePath, Arguments) {
            this.OutputDataReceived += CapturingProcess_OutputDataReceived;
            this.ErrorDataReceived += CapturingProcess_ErrorDataReceived;
        }
        
        public override void Start() {
            this.output.Clear();
            this.error.Clear();

            base.Start();
            base.BeginOutputReadLine();
            base.BeginErrorReadLine();
        }

        public override string GetOutputAsString() => String.Join(Environment.NewLine, this.output);
        public override string GetErrorAsString() => String.Join(Environment.NewLine, this.error);

        public override void Dispose() {
            this.OutputDataReceived -= this.CapturingProcess_OutputDataReceived;
            this.ErrorDataReceived -= this.CapturingProcess_ErrorDataReceived;

            base.Dispose();
        }

        private void CapturingProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            this.error.Add(e.Data);
        }

        private void CapturingProcess_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            this.output.Add(e.Data);
        }
    }
}
