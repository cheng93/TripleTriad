using System.Linq;
using FluentAssertions;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Steps;
using TripleTriad.Logic.Steps.Handlers;
using Xunit;

namespace TripleTriad.Logic.Tests.StepTests.HandlerTests
{
    public class CreateBoardHandlerTests
    {
        private static CreateBoardStep CreateStep()
            => new CreateBoardStep(new GameData());

        [Fact]
        public void Should_have_nine_tiles()
        {
            var subject = new CreateBoardHandler();
            var data = subject.Run(CreateStep());

            data.Tiles.Should().HaveCount(9);
            for (var i = 0; i < 9; i++)
            {
                data.Tiles.Should()
                    .Contain(t => t.TileId == i)
                    .Which
                    .Should()
                    .BeEquivalentTo(new Tile
                    {
                        TileId = i
                    });
            }
        }

        [Fact]
        public void Should_have_correct_log_entry()
        {
            var subject = new CreateBoardHandler();
            var data = subject.Run(CreateStep());

            data.Log.Last().Should().Be("Board initialized.");
        }
    }
}