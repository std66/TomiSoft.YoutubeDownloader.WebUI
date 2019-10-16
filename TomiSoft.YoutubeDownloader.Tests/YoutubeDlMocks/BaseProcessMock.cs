using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks {
    abstract class BaseProcessMock : IProcess, IProcessFactory {
        public enum Behavior {
            RunMainOnWaitForExit, RunMainOnStart
        }

        private IEnumerable<string> args;
        private readonly List<string> stdOut = new List<string>();
        private readonly List<string> stdErr = new List<string>();
        private readonly Behavior behavior;

        public event DataReceivedEventHandler OutputDataReceived;
        public event DataReceivedEventHandler ErrorDataReceived;
        public event EventHandler Exited;

        public int ExitCode { get; private set; }
        public bool ParametersPassedCorrectly { get; private set; }
        public bool ProcessStarted { get; private set; }
        public IReadOnlyList<string> StandardOutputLines => this.stdOut;
        public IReadOnlyList<string> StandardErrorLines => this.stdErr;
        public bool ExitedSuccessfully => ExitCode == 0;

        protected abstract bool CheckCmdLineArgs(IEnumerable<string> args);
        protected abstract int Main(string[] args);

        public IProcess Create(params string[] args) {
            this.ParametersPassedCorrectly = this.CheckCmdLineArgs(args);
            this.args = args;
            return this;
        }

        public IProcess Create(IEnumerable<string> args) {
            this.ParametersPassedCorrectly = this.CheckCmdLineArgs(args);
            this.args = args;
            return this;
        }

        public BaseProcessMock() : this(Behavior.RunMainOnWaitForExit) {

        }

        public BaseProcessMock(Behavior behavior) {
            this.behavior = behavior;
        }

        public virtual void Dispose() {

        }

        public string GetOutputAsString() {
            if (!this.ProcessStarted)
                throw new InvalidOperationException($"This process was not started yet. Something invoked {nameof(GetOutputAsString)} before even starting the process.");

            return String.Join("", this.stdOut);
        }

        public string GetErrorAsString() {
            if (!this.ProcessStarted)
                throw new InvalidOperationException($"This process was not started yet. Something invoked {nameof(GetErrorAsString)} before even starting the process.");

            return String.Join("", this.stdErr);
        }

        public void Start() {
            if (this.ProcessStarted)
                throw new InvalidOperationException("This process was already started. Shouldn't be reused.");

            this.ProcessStarted = true;

            if (behavior == Behavior.RunMainOnStart) {
                this.ExitCode = this.Main(this.args.ToArray());
                this.Exited?.Invoke(this, EventArgs.Empty);
            }
        }

        public void WaitForExit() {
            if (!this.ProcessStarted)
                throw new InvalidOperationException($"This process was not started yet. Something invoked {nameof(WaitForExit)} before even starting the process.");

            if (this.behavior == Behavior.RunMainOnWaitForExit) {
                this.ExitCode = this.Main(this.args.ToArray());
                this.Exited?.Invoke(this, EventArgs.Empty);
            }
        }

        protected void Write(string data) {
            this.stdOut.Add(data);
            this.OutputDataReceived?.Invoke(this, this.CreateMockDataReceivedEventArgs(data));
        }

        protected void WriteLine(string data) {
            Write($"{data}{Environment.NewLine}");
        }

        protected void ErrorWrite(string data) {
            this.stdErr.Add(data);
            this.ErrorDataReceived?.Invoke(this, this.CreateMockDataReceivedEventArgs(data));
        }

        protected void ErrorWriteLine(string data) {
            ErrorWrite($"{data}{Environment.NewLine}");
        }

        private DataReceivedEventArgs CreateMockDataReceivedEventArgs(string TestData) {
            if (TestData == null)
                throw new ArgumentNullException(nameof(TestData));

            DataReceivedEventArgs MockEventArgs =
                (DataReceivedEventArgs)System.Runtime.Serialization.FormatterServices
                 .GetUninitializedObject(typeof(DataReceivedEventArgs));

            FieldInfo[] EventFields = typeof(DataReceivedEventArgs)
                .GetFields(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly);

            if (EventFields.Count() > 0) {
                EventFields[0].SetValue(MockEventArgs, TestData);
            }
            else {
                throw new ApplicationException(
                    "Failed to find _data field!");
            }

            return MockEventArgs;

        }
    }
}
