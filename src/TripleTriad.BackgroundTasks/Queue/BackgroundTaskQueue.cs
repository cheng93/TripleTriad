using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TripleTriad.BackgroundTasks.Queue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<object> workItems = new ConcurrentQueue<object>();
        private SemaphoreSlim signal = new SemaphoreSlim(0);

        public async Task<object> DequeueAsync(CancellationToken cancellationToken)
        {
            await this.signal.WaitAsync(cancellationToken);
            this.workItems.TryDequeue(out var queueItem);

            return queueItem;
        }

        public void QueueBackgroundTask(object queueItem)
        {
            if (queueItem == null)
            {
                throw new ArgumentNullException(nameof(queueItem));
            }

            this.workItems.Enqueue(queueItem);
            this.signal.Release();
        }
    }
}