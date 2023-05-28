using System.IO;
using System.Threading.Tasks;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Dependencies.IO {
	public interface IFile {
		string Path { get; }
		bool Delete();
		Stream CreateReadStream();
		Task<string> ReadContentsAsStringAsync();
	}
}