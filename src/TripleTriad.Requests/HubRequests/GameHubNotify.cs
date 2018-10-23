using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
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

                var message = new
                {
                };

                if (game.Status == GameStatus.InProgress)
                {
                    await this.SendMessage(request, new
                    {
                        GameId = request.GameId,
                        Status = game.Status.ToString(),
                        Log = gameData.Log,
                        PlayerOneTurn = gameData.PlayerOneTurn,
                        PlayerOneWonCoinToss = gameData.PlayerOneWonCoinToss,
                        Tiles = gameData.Tiles
                    });
                }
                else if (game.Status == GameStatus.Finished)
                {
                    await this.SendMessage(request, new
                    {
                        GameId = request.GameId,
                        Status = game.Status.ToString(),
                        Log = gameData.Log,
                        PlayerOneTurn = gameData.PlayerOneTurn,
                        PlayerOneWonCoinToss = gameData.PlayerOneWonCoinToss,
                        Tiles = gameData.Tiles,
                        Result = gameData.Result
                    });
                }
                else
                {
                    await this.SendMessage(request, new
                    {
                        GameId = request.GameId,
                        Status = game.Status.ToString()
                    });
                }

                return new Unit();
            }

            private async Task SendMessage(TRequest request, object message)
            {
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                serializerSettings.Converters.Add(new StringEnumConverter());
                await this.GetGameClient(request).Send(JsonConvert.SerializeObject(message, serializerSettings));
            }

            protected abstract IGameClient GetGameClient(TRequest request);
        }
    }
}