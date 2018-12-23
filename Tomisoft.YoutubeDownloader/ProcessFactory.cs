﻿using System.Collections.Generic;

namespace TomiSoft.YoutubeDownloader {
    public class ProcessFactory : IProcessFactory {
        private readonly string ExecutablePath;

        public ProcessFactory(IYoutubeDlConfiguration Configuration) {
            this.ExecutablePath = Configuration.ExecutablePath;
        }

        public IProcess Create(params string[] args) {
            return new CapturingProcess(this.ExecutablePath, args);
        }

        public IProcess Create(IEnumerable<string> args) {
            return new CapturingProcess(this.ExecutablePath, args);
        }
    }
}
