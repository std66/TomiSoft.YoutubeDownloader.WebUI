using System.Threading;
using System.Threading.Tasks;

namespace TomiSoft.Common.Hosting {
    public interface IQueueService<T> {
        T Dequeue(CancellationToken cancellationToken);
        void Enqueue(T item);
        Task WaitForAvailableItemAsync(CancellationToken cancellationToken);
    }
}