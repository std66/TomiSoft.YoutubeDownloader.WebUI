using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TomiSoft.Common.Hosting {
    public class QueueService<T> : IQueueService<T> {
        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(0);

        public void Enqueue(T item) {
            this.queue.Enqueue(item);
            this.semaphore.Release();
        }

        public Task WaitForAvailableItemAsync(CancellationToken cancellationToken) {
            if (queue.Count == 0)
                return semaphore.WaitAsync(cancellationToken);

            return Task.CompletedTask;
        }

        public T Dequeue(CancellationToken cancellationToken) {
            if (this.queue.TryDequeue(out T result))
                return result;

            return default(T);
        }
    }
}
