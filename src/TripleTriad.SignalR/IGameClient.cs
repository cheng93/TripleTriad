using System.Threading;
using System.Threading.Tasks;

namespace TripleTriad.SignalR
{
    public interface IGameClient
    {
        Task Send(GameDataMessage message, CancellationToken cancellationToken);
    }
}