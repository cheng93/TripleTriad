using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TripleTriad.Common;

namespace TripleTriad.SignalR
{
    [Authorize(Policy = Constants.TripleTriad)]
    public class GameHub : Hub<IGameClient>
    {
        public const string Lobby = "lobby";

        private readonly IConnectionIdStore store;

        public GameHub(IConnectionIdStore store)
        {
            this.store = store;
        }

        public async Task JoinLobby()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Lobby);
        }
        public async Task ViewGame(int gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            await this.store.Add(new ConnectionIdStoreEntry(Context.UserIdentifier, Context.ConnectionId));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await this.store.Remove(new ConnectionIdStoreEntry(Context.UserIdentifier, Context.ConnectionId));
            await base.OnDisconnectedAsync(exception);
        }
    }
}