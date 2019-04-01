using System.Threading.Tasks;
using MediatR;

namespace TripleTriad.Requests.Messages
{
    public static class GameList
    {
        public class MessageData : IMessageData { }

        public class MessageFactory : MessageFactory<MessageData>
        {
            private readonly IMediator mediator;

            public MessageFactory(IMediator mediator)
            {
                this.mediator = mediator;
            }

            protected async override Task<Message> GetMessage(MessageData data)
            {
                var response = await this.mediator.Send(new GameRequests.GameList.Request());

                return new Message
                {
                    Type = nameof(GameList),
                    Data = response.GameIds
                };
            }
        }
    }
}