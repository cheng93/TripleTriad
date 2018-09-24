using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using TripleTriad.BackgroundTasks.Queue;

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

        protected override Func<CancellationToken, Task> CreateTask(TRequest request, TResponse response)
        {
            return (cancellationToken)
                => this.mediator.Send(
                    this.CreateQueueRequest(request, response),
                    cancellationToken);
        }
    }
}