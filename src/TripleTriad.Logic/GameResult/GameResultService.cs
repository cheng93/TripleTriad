using System.Linq;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.GameResult
{
    public class GameResultService : IGameResultService
    {
        public Result? GetResult(GameData gameData)
        {
            if (!gameData.HostWonCoinToss.HasValue
                || (gameData.Tiles?.Any(x => x.Card == null) ?? true))
            {
                return null;
            }

            var host = gameData.Tiles.Count(x => x.Card.IsHost);
            var playerTwo = gameData.Tiles.Count(x => !x.Card.IsHost);

            if (gameData.HostWonCoinToss.Value)
            {
                playerTwo += 1;
            }
            else
            {
                host += 1;
            }

            return host > playerTwo
                ? Result.HostWin
                : host < playerTwo
                    ? Result.PlayerTwoWin
                    : Result.Tie;
        }
    }
}