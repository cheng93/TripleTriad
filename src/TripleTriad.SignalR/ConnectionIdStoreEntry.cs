namespace TripleTriad.SignalR
{
    public class ConnectionIdStoreEntry
    {
        public ConnectionIdStoreEntry(string userId, string connectionId)
        {
            this.UserId = userId;
            this.ConnectionId = connectionId;

        }

        public string UserId { get; }
        public string ConnectionId { get; }
    }
}