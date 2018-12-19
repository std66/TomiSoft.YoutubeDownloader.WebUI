using System.Collections.Generic;

namespace Tomisoft.YoutubeDownloader {
    public interface IProcessFactory {
        IProcess Create(params string[] args);
        IProcess Create(IEnumerable<string> args);
    }
}
