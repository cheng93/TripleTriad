using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TripleTriad.SignalR.Constants;

namespace TripleTriad.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetPlayerId(this HttpContext context)
        {
            var claim = context
                .User
                .Claims
                .FirstOrDefault(x => x.Type == ClaimConstants.PlayerId);

            return new Guid(claim.Value);
        }
    }
}