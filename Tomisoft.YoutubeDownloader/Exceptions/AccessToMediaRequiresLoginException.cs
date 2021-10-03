using System;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader.Exceptions
{
    public class AccessToMediaRequiresLoginException : MediaInformationExtractException
    {
        public AccessToMediaRequiresLoginException(IProcess Process, Uri MediaUri) : base(Process, MediaUri)
        {
        }
    }
}
