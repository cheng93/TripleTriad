using System.Linq;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Extensions;

namespace TripleTriad.Logic.Steps.Handlers
{
    public class CreateBoardHandler : IStepHandler<CreateBoardStep>
    {
        public GameData Run(CreateBoardStep step)
        {
            step.Data.Tiles = Enumerable.Range(0, 9)
                .Select(x => new Tile
                {
                    TileId = x
                });

            step.Log("Board initialized.");

            return step.Data;
        }
    }
}