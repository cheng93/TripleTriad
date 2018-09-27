using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TripleTriad.Data;
using TripleTriad.Logic.Entities;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Requests.Response;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameNotify
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
                    Log = gameData.Log,
                    PlayerOneTurn = gameData.PlayerOneTurn,
                    PlayerOneWonCoinToss = gameData.PlayerOneWonCoinToss,
                    Tiles = gameData.Tiles,
                    Result = gameData.Result
                };

                await this.GetGameClient(request).Send(message);

                return new Unit();
            }

            protected abstract IGameClient GetGameClient(TRequest request);
        }
    }
}