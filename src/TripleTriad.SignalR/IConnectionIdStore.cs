using System.Collections.Generic;
using System.Threading.Tasks;

namespace TripleTriad.SignalR
{
    public interface IConnectionIdStore
    {
        Task Add(ConnectionIdStoreEntry entry);

        Task Remove(ConnectionIdStoreEntry entry);

        Task<IEnumerable<string>> GetConnectionIds(string userId);
    }
}