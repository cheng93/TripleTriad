using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.Notifications
{
    public class LobbyNotification : INotification { }

    public class LobbyNotificationHandler : INotificationHandler<LobbyNotification>
    {
        private readonly IMediator mediator;
        private readonly IMessageFactory<Messages.GameList.MessageData> messageFactory;

        public LobbyNotificationHandler(
            IMediator mediator,
            IMessageFactory<Messages.GameList.MessageData> messageFactory)
        {
            this.messageFactory = messageFactory;
            this.mediator = mediator;
        }

        public async Task Handle(LobbyNotification notification, CancellationToken cancellationToken)
        {
            var lobbyRequest = new HubGroupNotify.Request
            {
                Group = GameHub.Lobby,
                Message = await this.messageFactory.Create(new Messages.GameList.MessageData())
            };

            await this.mediator.Send(lobbyRequest);
        }
    }
}