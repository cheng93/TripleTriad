using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.Response;

namespace TripleTriad.Requests.Pipeline
{
    public abstract class NotificationSenderPostProcessor<TRequest, TResponse>
        : IRequestPostProcessor<TRequest, TResponse>
    {
        protected IBackgroundTaskQueue Queue { get; }

        public NotificationSenderPostProcessor(IBackgroundTaskQueue queue)
        {
            this.Queue = queue;
        }

        public Task Process(TRequest request, TResponse response)
        {
            if (response is ISendNotificationResponse r && r.QueueTask)
            {
                this.SendNotifications(request, response);
            }
            return Task.CompletedTask;
        }

        protected abstract void SendNotifications(TRequest request, TResponse response);
    }
}