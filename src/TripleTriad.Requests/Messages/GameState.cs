using System;
using System.Threading.Tasks;
using TripleTriad.Data;
using TripleTriad.Requests.Extensions;
using TripleTriad.Requests.Messages.GameStateDataStrategies;

namespace TripleTriad.Requests.Messages
{
    public static class GameState
    {
        public class MessageData : IMessageData
        {
            public int GameId { get; set; }

            public Guid? PlayerId { get; set; }
        }

        public class MessageFactory : MessageFactory<MessageData>
        {
            private readonly TripleTriadDbContext context;
            private readonly IGameStateDataStrategyFactory dataStrategyFactory;

            public MessageFactory(
                TripleTriadDbContext context,
                IGameStateDataStrategyFactory dataStrategyFactory)
            {
                this.context = context;
                this.dataStrategyFactory = dataStrategyFactory;
            }

            protected async override Task<Message> GetMessage(MessageData data)
            {
                var game = await this.context.Games.GetGameOrThrowAsync(data.GameId, default);
                var strategy = this.dataStrategyFactory.GetStrategy(game);

                return new Message
                {
                    Type = nameof(GameState),
                    Data = strategy.GetData(game, data.PlayerId)
                };
            }
        }
    }
}