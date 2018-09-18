using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Commands.GuestPlayer;
using TripleTriad.Data;
using TripleTriad.Data.Entities;

namespace TripleTriad.Web.Middlewares
{
    public class PlayerIdMiddleware
    {
        private readonly RequestDelegate next;

        public PlayerIdMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, IMediator mediator)
        {
            var playerId = httpContext.Session.GetString("PlayerId");

            if (!Guid.TryParse(playerId, out _))
            {
                var createPlayerResponse = await mediator.Send(new GuestPlayerCreate.Command());

                httpContext.Session.SetString("PlayerId", createPlayerResponse.PlayerId.ToString());
            }

            await next(httpContext);
        }
    }
}