using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TripleTriad.Data.Entities;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Requests.Messages.GameStateDataStrategies
{
    public static class ChooseCards
    {
        public class GameStateDataStrategy : IGameStateDataStrategy
        {
            public GameStateData GetData(Game game, Guid? playerId)
            {
                var data = new Data
                {
                    GameId = game.GameId,
                    Status = game.Status.ToString()
                };

                if (playerId.HasValue)
                {
                    var gameData = JsonConvert.DeserializeObject<GameData>(game.Data);
                    var cards = playerId == game.HostId
                        ? gameData.HostCards
                        : gameData.ChallengerCards;

                    if (cards != null)
                    {
                        data.SelectedCards = cards.Select(x => x.Name);
                    }
                }

                return data;
            }
        }

        public class Data : GameStateData
        {
            public IEnumerable<string> SelectedCards { get; set; } = Enumerable.Empty<string>();
        }
    }
}