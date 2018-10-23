using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TripleTriad.BackgroundTasks.Queue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> workItems
            = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim signal = new SemaphoreSlim(0);

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await this.signal.WaitAsync(cancellationToken);
            this.workItems.TryDequeue(out var task);

            return task;
        }

        public void QueueBackgroundTask(Func<CancellationToken, Task> task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            this.workItems.Enqueue(task);
            this.signal.Release();
        }
    }
}