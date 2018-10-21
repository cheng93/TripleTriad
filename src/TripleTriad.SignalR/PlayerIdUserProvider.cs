using System.Linq;
using Microsoft.AspNetCore.SignalR;
using TripleTriad.SignalR.Constants;

namespace TripleTriad.SignalR
{
    public class PlayerIdUserProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection
                .User
                .Claims
                .First(x => x.Type == ClaimConstants.PlayerId)
                .Value;
        }
    }
}