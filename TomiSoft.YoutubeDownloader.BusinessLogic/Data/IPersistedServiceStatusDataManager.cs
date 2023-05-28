using System;
using System.Threading.Tasks;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Data {
	public interface IPersistedServiceStatusDataManager {
		/// <summary>
		///     Gets the time when the last update was performed.
		/// </summary>
		/// <returns>
		///     A <see cref="Task{TResult}"/> that represents the asynchronous operation.
		///     
		///     A <see cref="Nullable{T}"/> of <see cref="DateTime"/> is returned representing the time of the last update.
		///     Null is returned if this time is unknown.
		/// </returns>
		Task<DateTime?> GetLastUpdateTimeAsync();

		/// <summary>
		///     Saves the time of the last update.
		/// </summary>
		/// <param name="lastUpdateTime">The time of the last update.</param>
		/// <returns>
		///     A <see cref="Task{TResult}"/> that represents the asynchronous operation.
		/// </returns>
		Task SaveLastUpdateTimeAsync(DateTime lastUpdateTime);
	}
}