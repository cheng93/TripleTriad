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

        public Task Process(TRequest request, TResponse response)
        {
            if (response.QueueTask)
            {
                this.queue.QueueBackgroundTask(response);
            }
            return Task.CompletedTask;
        }
    }
}