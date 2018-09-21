using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TripleTriad.Data;
using TripleTriad.Data.Entities;
using TripleTriad.Data.Enums;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.CoinToss;
using TripleTriad.Requests.GameRequests.Exceptions;
using TripleTriad.Requests.Pipeline;
using TripleTriad.Logic.Extensions;

namespace TripleTriad.Requests.GameRequests
{
    public static class GameStart
    {
        public class Response
        {
            public int GameId { get; set; }

            public Guid StartPlayerId { get; set; }
        }

        public class Request : IRequest<Response>
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

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly TripleTriadDbContext context;
            private readonly ICoinTossStep coinTossStep;

            public RequestHandler(TripleTriadDbContext context, ICoinTossStep coinTossStep)
            {
                this.context = context;
                this.coinTossStep = coinTossStep;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Game game;
                var gameExists = await this.context.Games.AnyAsync(
                    x => x.GameId == request.GameId,
                    cancellationToken);
                if (!gameExists)
                {
                    throw new GameNotFoundException(request.GameId);
                }
                try
                {
                    game = await this.context.Games.SingleAsync(
                    x => x.GameId == request.GameId
                        && x.Status == GameStatus.Waiting,
                    cancellationToken);
                }
                catch (InvalidOperationException)
                {
                    throw new GameHasStartedException(request.GameId);
                }

                var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);
                gameData = this.coinTossStep.TossCoin(gameData, game.PlayerOne.DisplayName, game.PlayerTwo.DisplayName);

                game.Status = GameStatus.InProgress;
                game.Data = gameData.ToJson();

                await this.context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    GameId = request.GameId,
                    StartPlayerId = gameData.PlayerOneTurn.Value
                        ? game.PlayerOneId
                        : game.PlayerTwoId.Value
                };
            }
        }
    }
}