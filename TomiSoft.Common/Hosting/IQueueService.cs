using System.Threading;
using System.Threading.Tasks;

namespace TomiSoft.Common.Hosting {
    public interface IQueueService<T> {
        bool TryDequeue(out T result);
        void Enqueue(T item);
        Task WaitForAvailableItemAsync(CancellationToken cancellationToken);
    }
}