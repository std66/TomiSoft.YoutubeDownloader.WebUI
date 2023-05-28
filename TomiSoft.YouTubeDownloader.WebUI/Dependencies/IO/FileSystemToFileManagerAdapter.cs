using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.BusinessLogic.Dependencies.IO;

namespace TomiSoft.YouTubeDownloader.WebUI.Dependencies.IO {
	internal class FileSystemToFileManagerAdapter : IFileManager {
		private readonly IFileSystem fileSystem;

		public FileSystemToFileManagerAdapter(IFileSystem fileSystem) {
			this.fileSystem = fileSystem;
		}

		public async Task<(bool, YoutubeDownloader.BusinessLogic.Dependencies.IO.IFile)> TryCreateFileAsync(string path, string contents, bool allowOverwrite) {
			if (this.TryGetFile(path, out YoutubeDownloader.BusinessLogic.Dependencies.IO.IFile file)) {
				if (!allowOverwrite)
					return (false, null);

				file.Delete();
			}

			try {
				await fileSystem.File.WriteAllTextAsync(path, contents);
			}
			catch (Exception) {
				return (false, null);
			}

			return (true, file);
		}

		public bool TryGetFile(string path, out YoutubeDownloader.BusinessLogic.Dependencies.IO.IFile file) {
			IFileInfo fileInfo = fileSystem.FileInfo.New(path);
			if (!fileInfo.Exists) {
				file = null;
				return false;
			}

			file = new File(fileSystem, fileInfo.FullName);
			return true;
		}
	}
}