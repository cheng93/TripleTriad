using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using TripleTriad.BackgroundTasks.Queue;

namespace TripleTriad.Requests.Pipeline
{
    public interface IBackgroundQueueResponse
    {
        bool QueueTask { get; set; }
    }

    public abstract class BackgroundQueuePostProcessor<TRequest, TResponse>
        : IRequestPostProcessor<TRequest, TResponse>
        where TResponse : IBackgroundQueueResponse
    {
        private readonly IBackgroundTaskQueue queue;

        public BackgroundQueuePostProcessor(IBackgroundTaskQueue queue)
        {
            this.queue = queue;
        }

        public Task Process(TRequest request, TResponse response)
        {
            if (response.QueueTask)
            {
                var task = this.CreateTask(request, response);
                this.queue.QueueBackgroundTask(task);
            }
            return Task.CompletedTask;
        }

        protected abstract Func<CancellationToken, Task> CreateTask(TRequest request, TResponse response);
    }
}