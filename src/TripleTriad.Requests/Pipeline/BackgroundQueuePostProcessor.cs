using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using TripleTriad.BackgroundTasks.Queue;

namespace TripleTriad.Requests.Pipeline
{
    public abstract class BackgroundQueuePostProcessor<TRequest, TResponse, TQueueRequest, TQueueResponse>
        : IRequestPostProcessor<TRequest, TResponse>
        where TQueueRequest : IRequest<TQueueResponse>
    {
        private readonly IBackgroundTaskQueue queue;
        private readonly IMediator mediator;

        public BackgroundQueuePostProcessor(IBackgroundTaskQueue queue, IMediator mediator)
        {
            this.queue = queue;
            this.mediator = mediator;
        }

        public Task Process(TRequest request, TResponse response)
        {
            var task = this.CreateTask(request, response);
            this.queue.QueueBackgroundTask(task);
            return Task.CompletedTask;
        }

        protected abstract TQueueRequest CreateQueueRequest(TRequest request, TResponse response);

        private Func<CancellationToken, Task> CreateTask(TRequest request, TResponse response)
        {
            return (cancellationToken)
                => this.mediator.Send(
                    this.CreateQueueRequest(request, response),
                    cancellationToken);
        }
    }
}