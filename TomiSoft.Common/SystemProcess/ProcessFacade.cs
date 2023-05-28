using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TomiSoft.Common.SystemProcess {
	public abstract class ProcessFacade : IProcess {
		private Process process;

		private readonly IEnumerable<string> Arguments;
		private readonly string ExecutablePath;

		public int ExitCode => process.HasExited ? process.ExitCode : throw new InvalidOperationException("The process has not exited yet");
		public bool ExitedSuccessfully => process.HasExited && process.ExitCode == 0;
		public abstract IReadOnlyList<string> StandardOutputLines { get; }
		public abstract IReadOnlyList<string> StandardErrorLines { get; }

		public string CommandLine => $"{ExecutablePath} {string.Join(" ", Arguments)}";

		public event DataReceivedEventHandler OutputDataReceived {
			add {
				process.OutputDataReceived += value;
			}
			remove {
				process.OutputDataReceived -= value;
			}
		}

		public event DataReceivedEventHandler ErrorDataReceived {
			add {
				process.ErrorDataReceived += value;
			}
			remove {
				process.ErrorDataReceived -= value;
			}
		}

		public event EventHandler Exited {
			add {
				process.Exited += value;
			}
			remove {
				process.Exited -= value;
			}
		}

		public ProcessFacade(string ExecutablePath, IEnumerable<string> Arguments) {
			this.ExecutablePath = ExecutablePath;
			this.Arguments = new List<string>(Arguments ?? Array.Empty<string>());

			this.process = new Process() {
				StartInfo = this.CreateProcess(),
				EnableRaisingEvents = true
			};
		}

		public abstract string GetErrorAsString();
		public abstract string GetOutputAsString();

		public virtual void Start() => process.Start();
		public virtual void WaitForExit() => process.WaitForExit();
		public virtual void Dispose() => process.Dispose();

		public void BeginOutputReadLine() => process.BeginOutputReadLine();
		public void BeginErrorReadLine() => process.BeginErrorReadLine();

		private ProcessStartInfo CreateProcess() {
			return new ProcessStartInfo {
				CreateNoWindow = true,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				FileName = this.ExecutablePath,
				Arguments = String.Join(" ", this.Arguments),
				WorkingDirectory = Path.GetDirectoryName(this.ExecutablePath)
			};
		}
	}
}