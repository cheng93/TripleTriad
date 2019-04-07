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
            var challenger = gameData.Tiles.Count(x => !x.Card.IsHost);

            if (gameData.HostWonCoinToss.Value)
            {
                challenger += 1;
            }
            else
            {
                host += 1;
            }

            return host > challenger
                ? Result.HostWin
                : host < challenger
                    ? Result.ChallengerWin
                    : Result.Tie;
        }
    }
}