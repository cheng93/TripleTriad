using System.Linq;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.GameResult
{
    public class GameResultService : IGameResultService
    {
        public Result? GetResult(GameData gameData)
        {
            if (!gameData.PlayerOneWonCoinToss.HasValue
                || (gameData.Tiles?.Any(x => x.Card == null) ?? true))
            {
                return null;
            }

            var playerOne = gameData.Tiles.Count(x => x.Card.IsPlayerOne);
            var playerTwo = gameData.Tiles.Count(x => !x.Card.IsPlayerOne);

            if (gameData.PlayerOneWonCoinToss.Value)
            {
                playerTwo += 1;
            }
            else
            {
                playerOne += 1;
            }

            return playerOne > playerTwo
                ? Result.PlayerOneWin
                : playerOne < playerTwo
                    ? Result.PlayerTwoWin
                    : Result.Tie;
        }
    }
}