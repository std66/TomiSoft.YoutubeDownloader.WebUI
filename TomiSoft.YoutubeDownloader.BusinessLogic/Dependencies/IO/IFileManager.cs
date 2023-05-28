using System.Threading.Tasks;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Dependencies.IO {
	public interface IFileManager {
		/// <summary>
		///     Attempts to create a plain text file asynchronously with the given content.
		/// </summary>
		/// <param name="path">The path to the file. Use OS-specific delimiter (see <see cref="System.IO.Path.Combine(string[])"/>).</param>
		/// <param name="contents">The text that is going to be written into the file.</param>
		/// <param name="allowOverwrite">If true, an already existing file at the same <paramref name="path"/> is going to be overwritten. If false, the method will return false in such case.</param>
		/// <returns>
		///     A <see cref="Task{TResult}"/> that represents the asynchronous process.
		///     
		///     <para>Successful: True is returned if the file is successfully created, false otherwise.</para>
		///     <para>File: The resulted <see cref="IFile"/> instance representing that file, if exists. Null is returned otherwise.</para>
		/// </returns>
		Task<(bool Successful, IFile File)> TryCreateFileAsync(string path, string contents, bool allowOverwrite);

		/// <summary>
		///     If the file at a given <paramref name="path"/> exists, returns an <see cref="IFile"/> instance representing that file.
		/// </summary>
		/// <param name="path">The path to the file. Use OS-specific delimiter (see <see cref="System.IO.Path.Combine(string[])"/>).</param>
		/// <param name="file">The resulted <see cref="IFile"/> instance representing that file, if exists. Null is returned otherwise.</param>
		/// <returns>True if the file exists, false if not.</returns>
		bool TryGetFile(string path, out IFile file);
	}
}