using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TripleTriad.SignalR
{
    public class GameHub : Hub<IGameClient>
    {
        public async Task ViewGame(int gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }
    }
}