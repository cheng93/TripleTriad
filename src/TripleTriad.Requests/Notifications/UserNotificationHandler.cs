using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TripleTriad.Requests.Extensions;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;

namespace TripleTriad.Requests.Notifications
{
    public class UserNotification : INotification
    {
        public UserNotification(int gameId, Guid userId)
        {
            this.GameId = gameId;
            UserId = userId;
        }

        public int GameId { get; }
        public Guid UserId { get; }
    }

    public class UserNotificationHandler : INotificationHandler<UserNotification>
    {
        private readonly IMediator mediator;
        private readonly IMessageFactory<GameState.MessageData> messageFactory;

        public UserNotificationHandler(
            IMediator mediator,
            IMessageFactory<Messages.GameState.MessageData> messageFactory)
        {
            this.mediator = mediator;
            this.messageFactory = messageFactory;
        }

        public async Task Handle(UserNotification notification, CancellationToken cancellationToken)
        {
            var userRequest = new HubUserNotify.Request
            {
                UserId = notification.UserId,
                Message = await this.messageFactory.Create(
                    new Messages.GameState.MessageData
                    {
                        GameId = notification.GameId,
                        PlayerId = notification.UserId
                    })
            };

            await this.mediator.Send(userRequest);
        }
    }
}