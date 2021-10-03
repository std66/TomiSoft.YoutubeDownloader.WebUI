using System;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader.Exceptions
{
    public class PrivateMediaException : AccessToMediaRequiresLoginException
    {
        public PrivateMediaException(IProcess Process, Uri MediaUri) : base(Process, MediaUri)
        {
        }
    }
}
