using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TripleTriad.Data;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Entities;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.HubRequests
{
    public static class GameHubNotify
    {
        public class Request : IRequest
        {
            public int GameId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                base.RuleFor(x => x.GameId).NotEmpty();
            }
        }

        public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest, Unit>
            where TRequest : Request
        {
            private readonly TripleTriadDbContext dbContext;

            protected RequestHandler(
                TripleTriadDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
            {
                var game = await this.dbContext.Games.SingleAsync(x => x.GameId == request.GameId, cancellationToken);
                var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);

                var message = new GameDataMessage
                {
                    GameId = request.GameId,
                    Status = game.Status.ToString()
                };

                if (game.Status == GameStatus.InProgress || game.Status == GameStatus.Finished)
                {
                    message.Log = gameData.Log;
                    message.PlayerOneTurn = gameData.PlayerOneTurn;
                    message.PlayerOneWonCoinToss = gameData.PlayerOneWonCoinToss;
                    message.Tiles = gameData.Tiles;
                    message.Result = gameData.Result;
                }

                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.Converters.Add(new StringEnumConverter());
                await this.GetGameClient(request).Send(JsonConvert.SerializeObject(message, serializerSettings));

                return new Unit();
            }

            protected abstract IGameClient GetGameClient(TRequest request);
        }
    }
}