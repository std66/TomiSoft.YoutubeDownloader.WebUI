using System.IO;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.BusinessLogic.Dependencies.IO;

namespace TomiSoft.YouTubeDownloader.WebUI.Dependencies.IO {
	internal class File : IFile {
		private System.IO.Abstractions.IFileSystem fileSystem;

		public File(System.IO.Abstractions.IFileSystem fileSystem, string path) {
			this.fileSystem = fileSystem;
			Path = path;
		}

		public string Path { get; }

		public Stream CreateReadStream() {
			return this.fileSystem.File.OpenRead(this.Path);
		}

		public bool Delete() {
			try {
				this.fileSystem.File.Delete(this.Path);
			}
			catch {
				return false;
			}

			return true;
		}

		public Task<string> ReadContentsAsStringAsync() {
			return fileSystem.File.ReadAllTextAsync(this.Path);
		}
	}
}