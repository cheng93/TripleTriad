using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.BackgroundTasks.Queue;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameMoveTests
{
    public class RoomNotifierTests
    {
        private static readonly int GameId = 2;
        private static readonly string Message = "Hello World";

        [Fact]
        public async Task Should_send_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<HubGroupNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var messageFactory = new Mock<IMessageFactory<Messages.GameState.MessageData>>();
            messageFactory
                .Setup(x => x.Create(
                    It.IsAny<Messages.GameState.MessageData>()))
                .ReturnsAsync(Message);

            var subject = new GameMove.RoomNotifier(
                mediator.Object,
                messageFactory.Object);

            var response = new GameMove.Response
            {
                GameId = GameId
            };

            await subject.Handle(response, default);

            mediator.Verify(x => x.Send(
                It.Is<HubGroupNotify.Request>(
                    y => y.Group == GameId.ToString()
                        && y.Message == Message),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_get_message_with_correct_parameters()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<HubGroupNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var messageFactory = new Mock<IMessageFactory<Messages.GameState.MessageData>>();
            messageFactory
                .Setup(x => x.Create(
                    It.IsAny<Messages.GameState.MessageData>()))
                .ReturnsAsync(Message);

            var subject = new GameMove.RoomNotifier(
                mediator.Object,
                messageFactory.Object);

            var response = new GameMove.Response
            {
                GameId = GameId
            };

            await subject.Handle(response, default);

            messageFactory.Verify(x => x.Create(
                It.Is<Messages.GameState.MessageData>(
                    y => y.GameId == GameId
                        && !y.PlayerId.HasValue)));
        }
    }
}