using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TripleTriad.SignalR
{
    public class ConnectionIdStore : IConnectionIdStore
    {
        private readonly Dictionary<string, List<string>> store = new Dictionary<string, List<string>>();

        private readonly object locker = new object();

        public Task Add(ConnectionIdStoreEntry entry)
        {
            lock (this.locker)
            {
                if (this.store.TryGetValue(entry.UserId, out var value))
                {
                    value.Add(entry.ConnectionId);
                }
                else
                {
                    this.store.Add(entry.UserId, new List<string> { entry.ConnectionId });
                }
            }

            return Task.CompletedTask;
        }

        public Task Remove(ConnectionIdStoreEntry entry)
        {
            lock (this.locker)
            {
                if (this.store.TryGetValue(entry.UserId, out var value)
                    && value.Any(x => x != entry.ConnectionId))
                {
                    value.Remove(entry.ConnectionId);
                }
                else
                {
                    this.store.Remove(entry.UserId);
                }
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetConnectionIds(string userId)
        {
            var connectionIds = this.store.TryGetValue(userId, out var value)
                ? value.ToList()
                : Enumerable.Empty<string>();

            return Task.FromResult(connectionIds);
        }
    }
}