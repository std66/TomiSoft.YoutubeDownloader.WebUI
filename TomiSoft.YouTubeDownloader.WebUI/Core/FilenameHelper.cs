using System;
using System.IO;
using System.Linq;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
	public class FilenameHelper {
		public static string RemoveNotAllowedChars(string Filename) {
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			return new string(Filename.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
		}
	}
}