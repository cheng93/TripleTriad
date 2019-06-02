using MediatR;

namespace TripleTriad.Requests.Response
{
    internal interface ISendNotificationResponse
    {
        bool QueueTask { get; }
    }
}