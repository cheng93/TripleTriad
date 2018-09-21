using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Steps
{
    public interface ICoinTossStep
    {
        GameData TossCoin(GameData gameData, string playerOneDisplay, string playerTwoDisplay);
    }
}