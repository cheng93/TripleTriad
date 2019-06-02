using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TripleTriad.Requests.GameRequests;

namespace TripleTriad.Requests.Notifications
{
    public class GameStartNotification : INotification
    {
        public GameStartNotification(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; }
    }

    public class GameStartNotificationHandler : INotificationHandler<GameStartNotification>
    {
        private readonly IMediator mediator;

        public GameStartNotificationHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task Handle(GameStartNotification notification, CancellationToken cancellationToken)
        {
            var request = new GameStart.Request
            {
                GameId = notification.GameId
            };

            this.mediator.Send(request);
            return Task.CompletedTask;
        }
    }
}