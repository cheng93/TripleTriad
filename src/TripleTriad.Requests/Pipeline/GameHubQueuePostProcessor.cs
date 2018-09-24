using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Data;
using TripleTriad.Logic.Entities;
using TripleTriad.Requests.Response;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.Pipeline
{
    public abstract class GameHubQueuePostProcessor<TRequest, TResponse>
        : BackgroundQueuePostProcessor<TRequest, TResponse>
        where TResponse : IBackgroundQueueResponse, IGameResponse
    {
        private readonly TripleTriadDbContext dbContext;
        private readonly IHubContext<GameHub, IGameClient> hubContext;

        public GameHubQueuePostProcessor(
            IBackgroundTaskQueue queue,
            TripleTriadDbContext dbContext,
            IHubContext<GameHub, IGameClient> hubContext)
            : base(queue)
        {
            this.dbContext = dbContext;
            this.hubContext = hubContext;
        }

        protected async override Task<Func<CancellationToken, Task>> CreateTaskAsync(TRequest request, TResponse response)
        {
            var game = await this.dbContext.Games.SingleAsync(x => x.GameId == response.GameId);
            var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);

            var message = new GameDataMessage
            {
                Log = gameData.Log,
                PlayerOneTurn = gameData.PlayerOneTurn,
                PlayerOneWonCoinToss = gameData.PlayerOneWonCoinToss,
                Tiles = gameData.Tiles,
                Result = gameData.Result
            };

            return (cancellationToken)
                => this.hubContext
                    .Clients
                    .Group(response.GameId.ToString())
                    .Send(message, cancellationToken);
        }
    }
}