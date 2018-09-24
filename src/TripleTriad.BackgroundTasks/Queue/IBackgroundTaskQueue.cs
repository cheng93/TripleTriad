using System;
using System.Threading;
using System.Threading.Tasks;

namespace TripleTriad.BackgroundTasks.Queue
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundTask(Func<CancellationToken, Task> task);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}