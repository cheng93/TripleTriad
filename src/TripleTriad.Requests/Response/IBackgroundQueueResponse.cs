namespace TripleTriad.Requests.Response
{
    public interface IBackgroundQueueResponse
    {
        bool QueueTask { get; set; }
    }
}