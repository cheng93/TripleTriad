using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TripleTriad.SignalR.Constants;

namespace TripleTriad.SignalR
{
    [Authorize(Policy = AuthConstants.TripleTriadScheme)]
    public class GameHub : Hub<IGameClient>
    {
        public const string Lobby = "lobby";

        public async Task JoinLobby()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Lobby);
        }
        public async Task ViewGame(int gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }
    }
}