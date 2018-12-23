using System.Collections.Generic;

namespace TomiSoft.YoutubeDownloader {
    public interface IProcessFactory {
        IProcess Create(params string[] args);
        IProcess Create(IEnumerable<string> args);
    }
}
