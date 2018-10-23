using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.GameResult
{
    public interface IGameResultService
    {
        Result? GetResult(GameData gameData);
    }
}