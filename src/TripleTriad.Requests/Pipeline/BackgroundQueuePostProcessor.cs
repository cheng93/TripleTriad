using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.Pipeline
{
    public abstract class BackgroundQueuePostProcessor<TRequest, TResponse>
        : IRequestPostProcessor<TRequest, TResponse>
        where TResponse : IBackgroundQueueResponse
    {
        private readonly IBackgroundTaskQueue queue;

        public BackgroundQueuePostProcessor(IBackgroundTaskQueue queue)
        {
            this.queue = queue;
        }

        public async Task Process(TRequest request, TResponse response)
        {
            if (response.QueueTask)
            {
                var task = await this.CreateTaskAsync(request, response);
                this.queue.QueueBackgroundTask(task);
            }
        }

        protected abstract Task<Func<CancellationToken, Task>> CreateTaskAsync(TRequest request, TResponse response);
    }
}