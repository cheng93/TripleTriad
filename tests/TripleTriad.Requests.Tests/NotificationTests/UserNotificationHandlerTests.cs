using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TripleTriad.Data.Entities;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Requests.Messages;
using TripleTriad.Requests.Notifications;
using Xunit;

namespace TripleTriad.Requests.Tests.NotificationTests
{
    public class UserNotificationHandlerTests
    {
        private const int GameId = 2;
        private const string Message = "Hello World";
        private static readonly Guid UserId = new Guid("70284446-0e66-498d-bbc8-c3a23793ccaa");

        private readonly Mock<IMessageFactory<Messages.GameState.MessageData>> messageFactory
            = new Mock<IMessageFactory<Messages.GameState.MessageData>>();
        private readonly Mock<IMediator> mediator = new Mock<IMediator>();

        public UserNotificationHandlerTests()
        {
            this.mediator
                .Setup(x => x.Send(
                    It.IsAny<HubUserNotify.Request>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            this.messageFactory
                .Setup(x => x.Create(
                    It.IsAny<Messages.GameState.MessageData>()))
                .ReturnsAsync(Message);
        }

        [Fact]
        public async Task Should_send_request()
        {
            var subject = new UserNotificationHandler(
                mediator.Object,
                messageFactory.Object);

            var notification = new UserNotification(GameId, UserId);

            await subject.Handle(notification, default);

            mediator.Verify(x => x.Send(
                It.Is<HubUserNotify.Request>(
                    y => y.UserId == UserId
                        && y.Message == Message),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_get_message_with_correct_parameters()
        {
            var subject = new UserNotificationHandler(
                mediator.Object,
                messageFactory.Object);

            var notification = new UserNotification(GameId, UserId);

            await subject.Handle(notification, default);

            messageFactory.Verify(x => x.Create(
                It.Is<Messages.GameState.MessageData>(
                    y => y.GameId == GameId
                        && y.PlayerId == UserId)));
        }
    }
}