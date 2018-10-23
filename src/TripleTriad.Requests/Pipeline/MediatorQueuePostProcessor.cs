using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.Pipeline
{
    public abstract class MediatorQueuePostProcessor<TRequest, TResponse, TQueueRequest, TQueueResponse>
        : BackgroundQueuePostProcessor<TRequest, TResponse>
        where TQueueRequest : IRequest<TQueueResponse>
        where TResponse : IBackgroundQueueResponse
    {
        private readonly IMediator mediator;

        public MediatorQueuePostProcessor(IBackgroundTaskQueue queue, IMediator mediator)
            : base(queue)
        {
            this.mediator = mediator;
        }

        protected abstract TQueueRequest CreateQueueRequest(TRequest request, TResponse response);

        protected override Task<Func<CancellationToken, Task>> CreateTaskAsync(TRequest request, TResponse response)
        {
            Func<CancellationToken, Task> task = (cancellationToken)
                => this.mediator.Send(
                    this.CreateQueueRequest(request, response),
                    cancellationToken);

            return Task.FromResult(task);
        }
    }
}