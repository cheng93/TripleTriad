using System;
using System.Threading;
using System.Threading.Tasks;

namespace TripleTriad.BackgroundTasks.Queue
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundTask(object queueItem);

        Task<object> DequeueAsync(CancellationToken cancellationToken);
    }
}