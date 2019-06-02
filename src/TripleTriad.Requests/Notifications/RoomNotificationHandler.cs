using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TripleTriad.Data;
using TripleTriad.Requests.Extensions;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.Requests.Response;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.Notifications
{
    public class RoomNotification : INotification
    {
        public RoomNotification(int gameId)
        {
            this.GameId = gameId;
        }

        public int GameId { get; }
    }

    public class RoomNotificationHandler : INotificationHandler<RoomNotification>
    {
        private readonly TripleTriadDbContext context;
        private readonly IMediator mediator;
        private readonly IMessageFactory<GameState.MessageData> messageFactory;
        private readonly IConnectionIdStore connectionIdStore;

        public RoomNotificationHandler(
            TripleTriadDbContext context,
            IMediator mediator,
            IMessageFactory<Messages.GameState.MessageData> messageFactory,
            IConnectionIdStore connectionIdStore)
        {
            this.context = context;
            this.mediator = mediator;
            this.messageFactory = messageFactory;
            this.connectionIdStore = connectionIdStore;
        }

        public async Task Handle(RoomNotification notification, CancellationToken cancellationToken)
        {
            var game = await this.context.Games.GetGameOrThrowAsync(notification.GameId, cancellationToken);
            var connectionIdTasks = Task.WhenAll(
                new[]
                {
                        game.HostId.ToString(),
                        game.ChallengerId.ToString()
                }
                .Select(x => this.connectionIdStore.GetConnectionIds(x)));

            var groupRequest = new HubGroupNotify.Request
            {
                Group = notification.GameId.ToString(),
                Message = await this.messageFactory.Create(
                    new Messages.GameState.MessageData
                    {
                        GameId = notification.GameId
                    }),
                Excluded = (await connectionIdTasks).SelectMany(x => x)
            };

            await this.mediator.Send(groupRequest);
        }
    }
}