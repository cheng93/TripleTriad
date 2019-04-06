using System.Linq;
using Microsoft.AspNetCore.SignalR;
using TripleTriad.Common;

namespace TripleTriad.SignalR
{
    public class PlayerIdUserProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection
                .User
                .Claims
                .First(x => x.Type == Constants.Claims.PlayerId)
                .Value;
        }
    }
}