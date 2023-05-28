using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TomiSoft.Common.SystemProcess {
	public class ProcessOutputDecorator : IProcess {

		private readonly IProcess decoratedProcess;
		private readonly Dictionary<string, Action<IReadOnlyDictionary<string, string>>> stdoutLineProcessors = new Dictionary<string, Action<IReadOnlyDictionary<string, string>>>();
		private readonly Dictionary<string, Action<IReadOnlyDictionary<string, string>>> stderrLineProcessors = new Dictionary<string, Action<IReadOnlyDictionary<string, string>>>();
		private readonly Dictionary<string, Regex> regexes = new Dictionary<string, Regex>();

		public int ExitCode => decoratedProcess.ExitCode;
		public bool ExitedSuccessfully => decoratedProcess.ExitedSuccessfully;
		public IReadOnlyList<string> StandardOutputLines => decoratedProcess.StandardOutputLines;
		public IReadOnlyList<string> StandardErrorLines => decoratedProcess.StandardErrorLines;

		public string CommandLine => decoratedProcess.CommandLine;

		public ProcessOutputDecorator(IProcess decoratedProcess) {
			this.decoratedProcess = decoratedProcess;
			this.decoratedProcess.OutputDataReceived += OnOutputLineReceived;
			this.decoratedProcess.ErrorDataReceived += OnErrorLineReceived;
		}

		private void OnErrorLineReceived(object sender, DataReceivedEventArgs e) {
			this.ProcessLine(e.Data, this.stderrLineProcessors);
		}

		private void OnOutputLineReceived(object sender, DataReceivedEventArgs e) {
			this.ProcessLine(e.Data, this.stdoutLineProcessors);
		}

		public void RegisterStdOutLineProcessor(string regex, Action<IReadOnlyDictionary<string, string>> action) {
			this.stdoutLineProcessors.Add(regex, action);
			RegisterRegex(regex);
		}

		private void RegisterRegex(string regex) {
			if (!regexes.ContainsKey(regex))
				regexes.Add(regex, new Regex(regex));
		}

		public void RegisterStdErrLineProcessor(string regex, Action<IReadOnlyDictionary<string, string>> action) {
			this.stderrLineProcessors.Add(regex, action);
			RegisterRegex(regex);
		}

		private void ProcessLine(string line, IReadOnlyDictionary<string, Action<IReadOnlyDictionary<string, string>>> targetDict) {
			if (line == null)
				return;

			foreach (var registeredAction in targetDict) {
				Regex regex = regexes[registeredAction.Key];
				Action<IReadOnlyDictionary<string, string>> action = registeredAction.Value;

				Match match = regex.Match(line);
				if (match.Success) {
					Dictionary<string, string> args = new Dictionary<string, string>();
					foreach (string capturingGroupName in regex.GetGroupNames()) {
						args.Add(capturingGroupName, match.Groups[capturingGroupName].Value);
					}

					action(args);
				}
			}
		}

		public event DataReceivedEventHandler OutputDataReceived {
			add {
				decoratedProcess.OutputDataReceived += value;
			}
			remove {
				decoratedProcess.OutputDataReceived -= value;
			}
		}

		public event DataReceivedEventHandler ErrorDataReceived {
			add {
				decoratedProcess.ErrorDataReceived += value;
			}
			remove {
				decoratedProcess.ErrorDataReceived -= value;
			}
		}

		public event EventHandler Exited {
			add {
				decoratedProcess.Exited += value;
			}
			remove {
				decoratedProcess.Exited -= value;
			}
		}

		public void Dispose() {
			decoratedProcess.ErrorDataReceived -= this.OnErrorLineReceived;
			decoratedProcess.OutputDataReceived -= this.OnErrorLineReceived;
			decoratedProcess.Dispose();
		}

		public string GetErrorAsString() => decoratedProcess.GetErrorAsString();
		public string GetOutputAsString() => decoratedProcess.GetOutputAsString();
		public void Start() => decoratedProcess.Start();
		public void WaitForExit() => decoratedProcess.WaitForExit();
	}
}