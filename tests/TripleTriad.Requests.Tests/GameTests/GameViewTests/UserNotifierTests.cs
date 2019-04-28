using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using Xunit;

namespace TripleTriad.Requests.Tests.GameTests.GameViewTests
{
    public class UserNotifierTests
    {
        private static readonly int GameId = 2;

        private static readonly Guid PlayerId = Guid.NewGuid();

        private static readonly string Message = "Hello World";

        [Fact]
        public async Task Should_send_request()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<HubUserNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var messageFactory = new Mock<IMessageFactory<Messages.GameState.MessageData>>();
            messageFactory
                .Setup(x => x.Create(
                    It.IsAny<Messages.GameState.MessageData>()))
                .ReturnsAsync(Message);

            var subject = new GameView.UserNotifier(
                mediator.Object,
                messageFactory.Object);

            var response = new GameView.Response
            {
                GameId = GameId,
                PlayerId = PlayerId
            };

            await subject.Handle(response, default);

            mediator.Verify(x => x.Send(
                It.Is<HubUserNotify.Request>(
                    y => y.UserId == PlayerId
                        && y.Message == Message),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_get_message_with_correct_parameters()
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(
                    It.IsAny<HubUserNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var messageFactory = new Mock<IMessageFactory<Messages.GameState.MessageData>>();
            messageFactory
                .Setup(x => x.Create(
                    It.IsAny<Messages.GameState.MessageData>()))
                .ReturnsAsync(Message);

            var subject = new GameView.UserNotifier(
                mediator.Object,
                messageFactory.Object);

            var response = new GameView.Response
            {
                GameId = GameId,
                PlayerId = PlayerId
            };

            await subject.Handle(response, default);

            messageFactory.Verify(x => x.Create(
                It.Is<Messages.GameState.MessageData>(
                    y => y.GameId == GameId
                        && y.PlayerId == PlayerId)));
        }
    }
}