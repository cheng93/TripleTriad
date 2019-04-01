using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TripleTriad.Requests.Pipeline
{
    public abstract class AsyncMediatorNotificationHandler<TNotification, TRequest, TResponse>
        : INotificationHandler<TNotification>
        where TNotification : INotification
        where TRequest : IRequest<TResponse>
    {
        private readonly IMediator mediator;

        public AsyncMediatorNotificationHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            await this.mediator.Send(await this.GetRequest(notification));
        }

        protected abstract Task<TRequest> GetRequest(TNotification notification);
    }

    public abstract class MediatorNotificationHandler<TNotification, TRequest, TResponse>
        : INotificationHandler<TNotification>
        where TNotification : INotification
        where TRequest : IRequest<TResponse>
    {
        private readonly IMediator mediator;

        public MediatorNotificationHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            await this.mediator.Send(this.GetRequest(notification));
        }

        protected abstract TRequest GetRequest(TNotification notification);
    }
}