using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TripleTriad.Data.Entities;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Requests.Messages.GameStateDataStrategies
{
    public static class InProgress
    {
        public class GameStateDataStrategy : IGameStateDataStrategy
        {
            public GameStateData GetData(Game game, Guid? playerId)
            {
                var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);

                var data = new Data
                {
                    GameId = game.GameId,
                    Status = game.Status.ToString(),
                    HostTurn = gameData.HostTurn.Value,
                    HostWonCoinToss = gameData.HostWonCoinToss.Value,
                    Tiles = gameData.Tiles
                };

                data.HostCards = gameData.HostCards
                    .Select(x => playerId == game.HostId ? x.Name : "Hidden");

                data.ChallengerCards = gameData.ChallengerCards
                    .Select(x => playerId == game.ChallengerId ? x.Name : "Hidden");

                return data;
            }
        }

        public class Data : GameStateData
        {
            public bool HostTurn { get; set; }

            public bool HostWonCoinToss { get; set; }

            public IEnumerable<string> HostCards { get; set; }

            public IEnumerable<string> ChallengerCards { get; set; }

            public IEnumerable<Tile> Tiles { get; set; }
        }
    }
}